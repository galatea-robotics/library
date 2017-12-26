﻿/*
 *  Copyright (c) 2013-2016, Cureos AB.
 *  All rights reserved.
 *  http://www.cureos.com
 *
 *	This file is part of Shim.Drawing.
 *
 *  Shim.Drawing is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Shim.Drawing is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with Shim.Drawing.  If not, see <http://www.gnu.org/licenses/>.
 */

using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ImagePixelEnumerator.Extensions;
using ImagePixelEnumerator.Helpers;
using ImagePixelEnumerator.Quantizers;
using ImagePixelEnumerator.Quantizers.DistinctCompetition;

namespace System.Drawing
{
    public sealed partial class Bitmap : Image
    {
        #region FIELDS

        private static readonly int ParallelTaskCount;

        private bool _disposed = false;

        private readonly int _width;
        private readonly int _height;
        private readonly int _stride;
        private readonly PixelFormat _pixelFormat;

        private readonly IntPtr _scan0;
        private readonly bool _freeScan0;

        private ColorPalette _palette;
        private IColorQuantizer _quantizer;

        #endregion

        #region CONSTRUCTORS

        static Bitmap()
        {
            ParallelTaskCount = Environment.ProcessorCount;
        }
        public Bitmap(int width, int height) : this(width, height, PixelFormat.Format32bppArgb)
        {
        }
        public Bitmap(int width, int height, PixelFormat pixelFormat)
        {
            _width = width;
            _height = height;
            _stride = GetStride(width, pixelFormat);
            _pixelFormat = pixelFormat;

            _scan0 = Marshal.AllocHGlobal(_stride * height);
            _freeScan0 = true;

            ResetPalette();
        }

        public Bitmap(int width, int height, int stride, PixelFormat pixelFormat, IntPtr scan0)
        {
            _width = width;
            _height = height;
            _stride = stride;
            _pixelFormat = pixelFormat;

            _scan0 = scan0;
            _freeScan0 = false;

            ResetPalette();
        }

        public Bitmap(Bitmap source)
        {
            this._width = source._width;
            this._height = source._height;
            this._stride = source._stride;
            this._pixelFormat = source._pixelFormat;

            var size = source._height * source._stride;
            var bytes = new byte[size];
            Marshal.Copy(source._scan0, bytes, 0, size);

            this._scan0 = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, this._scan0, size);

            this._freeScan0 = true;

            if (this._pixelFormat.IsIndexed())
            {
                this.SetPalette(new ColorPalette(source._palette.Entries));
            }
            else
            {
                this._quantizer = null;
                this._palette = null;
            }
        }

        public Bitmap(Bitmap source, int width, int height)
        {
            using (Graphics graphic = Graphics.FromImage(this))
            {
                graphic.DrawImage(source, 0, 0, width, height);
            }
        }

        ~Bitmap()
        {
            Dispose(false);
        }

        #endregion

        #region PROPERTIES

        public override PixelFormat PixelFormat
        {
            get { return _pixelFormat; }
        }

        public override int Width
        {
            get { return _width; }
        }

        public override int Height
        {
            get { return _height; }
        }

        public override Size Size
        {
            get
            {
                return new Size(_width, _height);
            }
        }

        public override ColorPalette Palette
        {
            get
            {
                UpdateQuantizer();
                return _palette;
            }
            set { SetPalette(value); }
        }

        public int HorizontalResolution { get; private set; }

        public int VerticalResolution { get; private set; }
        
        #endregion

        #region METHODS

        public override void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Bitmap Clone(Rectangle rect, PixelFormat format)
        {
            // TODO Account for rectangle
            return this.Clone(format);
        }

        public Bitmap Clone(PixelFormat pixelFormat)
        {
            if (pixelFormat == this._pixelFormat)
            {
                return new Bitmap(this);
            }

            List<Color> palette = null;

            // indexed formats require 2 passes - one more pass to determines colors for palette beforehand
            if (pixelFormat.IsIndexed())
            {
                var quantizer = new DistinctSelectionQuantizer();
                quantizer.Prepare(this);

                // Pass: scan
                ImageBuffer.ProcessPerPixel(this, null, ParallelTaskCount, (passIndex, pixel) =>
                {
                    var color = pixel.GetColor();
                    quantizer.AddColor(color, pixel.X, pixel.Y);
                    return false;
                });

                // determines palette
                palette = quantizer.GetPalette(pixelFormat.GetColorCount());
            }

            // Pass: apply
            Image result;
            ImageBuffer.TransformImagePerPixel(this, pixelFormat, palette, out result, null, ParallelTaskCount,
                (passIndex, sourcePixel, targetPixel) =>
                {
                    var color = sourcePixel.GetColor();
                    targetPixel.SetColor(color, _quantizer);
                    return true;
                });

            return (Bitmap)result;
        }

        internal Bitmap Clone()
        {
            return new Bitmap(this);
        }

        public Color GetPixel(int x, int y)
        {
            var color = new Color();
            ImageBuffer.ProcessPerPixel(this, new[] { new Point(x, y) }, 1, (passIndex, pixel) =>
            {
                color = pixel.GetColor();
                return false;
            });
            return color;
        }

        public void SetPixel(int x, int y, Color color)
        {
            UpdateQuantizer();
            ImageBuffer.ProcessPerPixel(this, new[] { new Point(x, y) }, 1, (passIndex, pixel) =>
            {
                pixel.SetColor(color, _quantizer);
                return true;
            });
        }

        public BitmapData LockBits(Rectangle rectangle, ImageLockMode readOnly, PixelFormat pixelFormat)
        {
            switch (pixelFormat)
            {
                case PixelFormat.Alpha:
                case PixelFormat.PAlpha:
                case PixelFormat.Indexed:
                case PixelFormat.Undefined:
                    throw new ArgumentException("LockBits method only applicable to pixel formats with prefix Format",
                        "pixelFormat");
            }
                
            if (!pixelFormat.Equals(_pixelFormat))
                throw new ArgumentException(String.Format("Bitmap.PixelFormat = {0}", _pixelFormat), "pixelFormat");

            return new BitmapData(_width, _height, _stride, _pixelFormat, _scan0);
        }

        public void UnlockBits(BitmapData sourceData)
        {
            // TODO Need to do anything here?
        }

        public void SetResolution(int horizontalResolution, int verticalResolution)
        {
            HorizontalResolution = horizontalResolution;
            VerticalResolution = verticalResolution;
        }

        private void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // Free managed disposable objects
            }

            // Free unmanaged objects
            if (_freeScan0) Marshal.FreeHGlobal(_scan0);

            _disposed = true;
        }

        private void UpdateQuantizer()
        {
            if (!_pixelFormat.IsIndexed() || _quantizer != null) return;

            _quantizer = new DistinctSelectionQuantizer();
            _quantizer.Prepare(this);

            // Pass: scan
            ImageBuffer.ProcessPerPixel(this, null, ParallelTaskCount, (passIndex, pixel) =>
            {
                var color = pixel.GetColor();
                _quantizer.AddColor(color, pixel.X, pixel.Y);
                return false;
            });
        }

        private void SetPalette(ColorPalette palette)
        {
            if (!_pixelFormat.IsIndexed())
                throw new InvalidOperationException("Not possible to set palette for non-indexed pixel formats.");
            if (palette == null || palette.Entries == null) throw new ArgumentNullException("palette");

            _palette = palette;
            _quantizer = null;
            UpdateQuantizer();
        }

        private void ResetPalette()
        {
            _palette = _pixelFormat.IsIndexed() ? new ColorPalette(new Color[_pixelFormat.GetColorCount()]) : null;
            _quantizer = null;
        }

        private static int GetStride(int width, PixelFormat format)
        {
            var bitsPerPixel = ((int)format & 0xff00) >> 8;
            var bytesPerPixel = (bitsPerPixel + 7) / 8;
            return 4 * ((width * bytesPerPixel + 3) / 4);
        }

        #endregion
    }
}
