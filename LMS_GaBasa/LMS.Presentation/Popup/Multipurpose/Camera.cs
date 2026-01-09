using System;
using System.Drawing;
using System.Linq;
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

        /// <summary>
        /// When ShowDialog returns DialogResult.OK this contains the scanned barcode text.
        /// </summary>
        public string ScannedCode { get; private set; }

        public Camera()
        {
            InitializeComponent();

            // Configure ZXing reader
            _reader = new BarcodeReader { AutoRotate = true };
            _reader.Options = new DecodingOptions
            {
                TryInverted = true,
                // You can restrict formats for higher performance, e.g.:
                // PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.CODE_128 }
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
                    // Ensure the preview control is visible and in front
                    if (VideoPlayer != null)
                    {
                        VideoPlayer.BringToFront();
                        VideoPlayer.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    // Show error so you can debug (device busy, driver, permission)
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
                // Assumes you placed a ComboBox named cmbDevices on the form in Designer.
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
                // no cameras found - ensure UI reflects that
                if (CmbBxDevices != null) CmbBxDevices.Items.Clear();
            }
        }

        private void StartCamera()
        {
            if (_videoDevices == null || _videoDevices.Count == 0) throw new InvalidOperationException("No camera devices available.");
            int idx = (CmbBxDevices != null && CmbBxDevices.SelectedIndex >= 0) ? CmbBxDevices.SelectedIndex : 0;
            var moniker = _videoDevices[idx].MonikerString;

            _videoDevice = new VideoCaptureDevice(moniker);
            _videoDevice.NewFrame += Video_NewFrame;

            // If you added a VideoSourcePlayer named videoSourcePlayer in Designer, set its VideoSource
            if (VideoPlayer != null)
            {
                VideoPlayer.VideoSource = _videoDevice;
                VideoPlayer.Start();
            }
            else
            {
                // fallback: start the device anyway so NewFrame fires
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
            // Clone the frame to avoid mutating original buffer
            Bitmap frame = null;
            try
            {
                frame = (Bitmap)eventArgs.Frame.Clone();

                // Try decode
                var result = _reader.Decode(frame);
                if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                {
                    var text = result.Text.Trim();

                    // Debounce identical repeated detections
                    var now = DateTime.UtcNow;
                    if (string.Equals(text, _lastCode) && (now - _lastDetected) < _debounce) return;

                    _lastDetected = now;
                    _lastCode = text;

                    // If Auto-accept is enabled (checkbox named chkAutoAccept) we can close immediately
                    BeginInvoke(new Action(() =>
                    {
                        ScannedCode = text;
                        // Optional UI feedback control (Label named lblResult)
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
                frame?.Dispose();
            }
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
