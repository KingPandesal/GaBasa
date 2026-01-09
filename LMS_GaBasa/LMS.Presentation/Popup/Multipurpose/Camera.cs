using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Common;

namespace LMS.Presentation.Popup.Multipurpose
{
    public partial class Camera : Form
    {
        private FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoDevice;
        private readonly BarcodeReader _reader;
        private DateTime _lastDetected = DateTime.MinValue;
        private string _lastCode = null;
        private readonly TimeSpan _debounce = TimeSpan.FromMilliseconds(1200);

        // throttle decode attempts to avoid overloading CPU (decode every 200ms)
        private DateTime _lastDecodeAttempt = DateTime.MinValue;
        private readonly TimeSpan _decodeInterval = TimeSpan.FromMilliseconds(200);

        /// <summary>
        /// When ShowDialog returns DialogResult.OK this contains the scanned barcode text.
        /// </summary>
        public string ScannedCode { get; private set; }

        public Camera()
        {
            InitializeComponent();

            // Configure ZXing reader with conservative, robust options for camera scanning
            _reader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,          // try extra effort for difficult shots
                    TryInverted = true,        // invert black/white if needed
                    PossibleFormats = new[]
                    {
                        BarcodeFormat.CODE_128, // match your generated barcodes
                        BarcodeFormat.CODE_39,
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.UPC_A,
                        BarcodeFormat.UPC_E
                    }.ToList()
                }
            };

            this.Load += Camera_Load;
            this.FormClosing += Camera_FormClosing;
        }

        private void Camera_Load(object sender, EventArgs e)
        {
            PopulateCameraList();

            // Auto-start preview for better UX; show helpful error messages if it fails.
            if (_videoDevices != null && _videoDevices.Count > 0)
            {
                try
                {
                    StartCamera();
                    if (LblResult != null) LblResult.Text = "Scanning...";
                    if (VideoPlayer != null)
                    {
                        VideoPlayer.BringToFront();
                        VideoPlayer.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to start camera preview: {ex.Message}", "Camera Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (LblResult != null) LblResult.Text = "Camera start failed";
                }
            }
            else
            {
                MessageBox.Show("No camera devices found. Please connect a webcam and try again.", "Camera", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (LblResult != null) LblResult.Text = "No camera found";
            }
        }

        private void PopulateCameraList()
        {
            try
            {
                _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                if (CmbBxDevices != null)
                {
                    CmbBxDevices.Items.Clear();
                    foreach (FilterInfo fi in _videoDevices)
                        CmbBxDevices.Items.Add(fi.Name);
                    if (CmbBxDevices.Items.Count > 0) CmbBxDevices.SelectedIndex = 0;
                }
            }
            catch
            {
                if (CmbBxDevices != null) CmbBxDevices.Items.Clear();
            }
        }

        private void StartCamera()
        {
            if (_videoDevices == null || _videoDevices.Count == 0) throw new InvalidOperationException("No camera devices available.");
            int idx = (CmbBxDevices != null && CmbBxDevices.SelectedIndex >= 0) ? CmbBxDevices.SelectedIndex : 0;
            var moniker = _videoDevices[idx].MonikerString;

            _videoDevice = new VideoCaptureDevice(moniker);

            // Optional: pick a reasonable resolution (choose highest available not exceeding 1280x720)
            try
            {
                var caps = _videoDevice.VideoCapabilities;
                if (caps != null && caps.Length > 0)
                {
                    var pick = caps
                        .OrderByDescending(vc => vc.FrameSize.Width * vc.FrameSize.Height)
                        .FirstOrDefault(vc => vc.FrameSize.Width <= 1280 && vc.FrameSize.Height <= 720)
                        ?? caps.OrderByDescending(vc => vc.FrameSize.Width * vc.FrameSize.Height).First();
                    _videoDevice.VideoResolution = pick;
                }
            }
            catch
            {
                // ignore resolution selection failures
            }

            _videoDevice.NewFrame += Video_NewFrame;

            // If you added a VideoSourcePlayer named VideoPlayer in Designer, set its VideoSource
            if (VideoPlayer != null)
            {
                VideoPlayer.VideoSource = _videoDevice;
                VideoPlayer.Start();
            }
            else
            {
                _videoDevice.Start();
            }
        }

        private void StopCamera()
        {
            try
            {
                if (_videoDevice != null)
                {
                    _videoDevice.NewFrame -= Video_NewFrame;
                }

                if (VideoPlayer != null)
                {
                    if (VideoPlayer.VideoSource != null)
                    {
                        VideoPlayer.SignalToStop();
                        VideoPlayer.WaitForStop();
                        VideoPlayer.VideoSource = null;
                    }
                }
                else if (_videoDevice != null)
                {
                    if (_videoDevice.IsRunning)
                    {
                        _videoDevice.SignalToStop();
                        _videoDevice.WaitForStop();
                    }
                }
            }
            catch { /* best-effort cleanup */ }
            finally
            {
                _videoDevice = null;
            }
        }

        private void Video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            // Throttle decoding attempts to reduce CPU
            if (DateTime.UtcNow - _lastDecodeAttempt < _decodeInterval) return;
            _lastDecodeAttempt = DateTime.UtcNow;

            Bitmap frame = null;
            Bitmap toDecode = null;
            try
            {
                // Clone the frame to avoid mutating original buffer
                frame = (Bitmap)eventArgs.Frame.Clone();

                // Optional: crop center region to speed decoding and avoid noisy borders
                Rectangle crop = GetCentralCrop(frame.Width, frame.Height, 0.7f); // 70% center area
                toDecode = (crop.Width == frame.Width && crop.Height == frame.Height) ? frame : frame.Clone(crop, frame.PixelFormat);

                // Decode synchronously on this NewFrame thread (AForge raises NewFrame on a background thread).
                // Using BarcodeReader.Decode(Bitmap) avoids BinaryBitmap <-> Bitmap conversion errors.
                Result result = null;
                try
                {
                    // BarcodeReader is safe to call here; if you observe thread-safety issues you can lock(_reader) or create a local reader instance.
                    result = _reader.Decode(toDecode);
                }
                catch
                {
                    // decode failure for this frame; ignore and continue
                    result = null;
                }

                if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                {
                    var text = result.Text.Trim();

                    // Debounce identical repeated detections
                    var now = DateTime.UtcNow;
                    if (string.Equals(text, _lastCode) && (now - _lastDetected) < _debounce)
                        return;

                    _lastDetected = now;
                    _lastCode = text;

                    // marshal to UI thread
                    BeginInvoke(new Action(() =>
                    {
                        ScannedCode = text;
                        if (LblResult != null) LblResult.Text = $"Scanned: {text}";

                        // Stop camera then close with OK
                        StopCamera();

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }));
                }
            }
            catch
            {
                // ignore per-frame errors
            }
            finally
            {
                // Dispose bitmaps created here. If toDecode references frame, disposing toDecode disposes frame as well.
                toDecode?.Dispose();
                // frame was either disposed by disposing toDecode (if same) or still needs disposing
                if (!object.ReferenceEquals(toDecode, frame))
                    frame?.Dispose();
            }
        }

        private Rectangle GetCentralCrop(int width, int height, float ratio)
        {
            // ratio between 0..1 determines fraction of frame used (0.7 -> 70% centered)
            if (ratio <= 0f || ratio >= 1f) return new Rectangle(0, 0, width, height);
            int w = (int)(width * ratio);
            int h = (int)(height * ratio);
            int x = (width - w) / 2;
            int y = (height - h) / 2;
            return new Rectangle(x, y, Math.Max(1, w), Math.Max(1, h));
        }

        // Example wired handlers for Start/Stop buttons (names should match your Designer)
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                StartCamera();
                if (LblResult != null) LblResult.Text = "Scanning...";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not start camera: {ex.Message}", "Camera", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopCamera();
            if (LblResult != null) LblResult.Text = "Stopped";
        }

        private void Camera_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopCamera();
        }
    }
}
