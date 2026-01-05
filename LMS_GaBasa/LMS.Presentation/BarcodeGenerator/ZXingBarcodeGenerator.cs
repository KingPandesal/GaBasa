using LMS.BusinessLogic.Services.BarcodeGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using ZXing;
using ZXing.Common;

namespace LMS.Presentation.BarcodeGenerator
{
    public class ZXingBarcodeGenerator : IBarcodeGenerator
    {
        private readonly BarcodeFormat _format;
        private readonly int _width;
        private readonly int _height;
        private readonly int _margin;
        private readonly string _outputFolder;
        private const int DbBarcodeMaxLength = 50;

        public ZXingBarcodeGenerator(string outputFolder,
            BarcodeFormat format = BarcodeFormat.CODE_128,
            int width = 300, int height = 100, int margin = 10)
        {
            _format = format;
            _width = width;
            _height = height;
            _margin = margin;
            _outputFolder = outputFolder ?? throw new ArgumentNullException(nameof(outputFolder));

            if (!Directory.Exists(_outputFolder))
                Directory.CreateDirectory(_outputFolder);
        }

        public IDictionary<string, string> GenerateMany(IEnumerable<string> texts)
        {
            var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            if (texts == null) return map;

            var writer = new BarcodeWriter
            {
                Format = _format,
                Options = new EncodingOptions
                {
                    Width = _width,
                    Height = _height,
                    Margin = _margin,
                    PureBarcode = true
                }
            };

            foreach (var t in texts)
            {
                if (string.IsNullOrWhiteSpace(t))
                {
                    map[t] = null;
                    continue;
                }

                try
                {
                    // Use accession as the base filename (sanitized). No GUID to keep names short.
                    string safe = MakeSafeFilename(t);
                    string filename = $"{safe}.png";
                    string fullPath = Path.Combine(_outputFolder, filename);

                    // Write/overwrite the file for the accession
                    using (Bitmap bmp = writer.Write(t))
                    {
                        bmp.Save(fullPath, System.Drawing.Imaging.ImageFormat.Png);
                    }

                    // Return relative path if under app base, otherwise filename.
                    string appRoot = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
                    string normalized = Path.GetFullPath(fullPath).TrimEnd(Path.DirectorySeparatorChar);

                    string rel;
                    if (normalized.StartsWith(appRoot, StringComparison.OrdinalIgnoreCase))
                    {
                        rel = normalized.Substring(appRoot.Length).TrimStart(Path.DirectorySeparatorChar)
                            .Replace(Path.DirectorySeparatorChar, '/');
                    }
                    else
                    {
                        rel = fullPath;
                    }

                    // Ensure value fits DB column. If too long, fall back to just the filename.
                    if (rel.Length > DbBarcodeMaxLength)
                    {
                        // If even filename is too long (unlikely), truncate filename to fit.
                        string justFile = filename;
                        if (justFile.Length > DbBarcodeMaxLength)
                        {
                            justFile = justFile.Substring(justFile.Length - DbBarcodeMaxLength);
                        }
                        map[t] = justFile;
                    }
                    else
                    {
                        map[t] = rel;
                    }
                }
                catch
                {
                    // On failure return null for this accession so caller can decide.
                    map[t] = null;
                }
            }

            return map;
        }

        private string MakeSafeFilename(string text)
        {
            if (string.IsNullOrEmpty(text)) return Guid.NewGuid().ToString("N");

            foreach (var c in Path.GetInvalidFileNameChars())
                text = text.Replace(c, '_');

            // Keep filename short: accession strings (e.g. BK-2026-0001) are short, but cap it.
            if (text.Length > 32) text = text.Substring(0, 32);
            return text;
        }
    }
}
