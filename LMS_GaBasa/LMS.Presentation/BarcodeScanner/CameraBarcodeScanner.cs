using System;
using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;
using ZXing;
using ZXing.Common;

namespace LMS.Presentation.BarcodeScanner
{
    public class CameraBarcodeScanner : IDisposable
    {
        private FilterInfoCollection _videoDevices;
        private VideoCaptureDevice _videoDevice;
        private volatile bool _scanning;
        private readonly BarcodeReader _reader;

        public event Action<string> BarcodeDetected;
        public bool IsRunning => _videoDevice != null && _videoDevice.IsRunning;

        public CameraBarcodeScanner()
        {
            // Use Options.TryInverted per ZXing API (TryInverted is now obsolete on BarcodeReader)
            _reader = new BarcodeReader { AutoRotate = true };
            _reader.Options = new DecodingOptions
            {
                TryInverted = true
            };
        }

        public void InitializeDefaultDevice()
        {
            _videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (_videoDevices.Count == 0) throw new InvalidOperationException("No video devices found.");
            _videoDevice = new VideoCaptureDevice(_videoDevices[0].MonikerString);
        }

        public void Start()
        {
            if (_videoDevice == null) InitializeDefaultDevice();
            if (_videoDevice == null) return;

            if (!_videoDevice.IsRunning)
            {
                _videoDevice.NewFrame += VideoDevice_NewFrame;
                _videoDevice.Start();
                _scanning = true;
            }
        }

        public void Stop()
        {
            _scanning = false;
            if (_videoDevice != null && _videoDevice.IsRunning)
            {
                _videoDevice.NewFrame -= VideoDevice_NewFrame;
                _videoDevice.SignalToStop();
                _videoDevice.WaitForStop();
            }
        }

        private void VideoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (!_scanning) return;

            Bitmap frame = null;
            try
            {
                // Clone the frame to avoid threading issues
                frame = (Bitmap)eventArgs.Frame.Clone();

                // Decode with ZXing
                var result = _reader.Decode(frame);
                if (result != null && !string.IsNullOrWhiteSpace(result.Text))
                {
                    // Raise event on thread pool to avoid blocking the NewFrame event
                    var text = result.Text;
                    BarcodeDetected?.Invoke(text);
                    // optionally stop after first detection:
                    // Stop();
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

        public void Dispose()
        {
            Stop();
            _videoDevice = null;
        }
    }
}
