/*
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

using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;

namespace System.Drawing
{
    using System.Drawing.Imaging;

    public sealed partial class Bitmap
    {
        #region METHODS

        public static Bitmap Create(Stream stream)
        {
            throw new NotImplementedException("PCL");
        }

        public static Bitmap FromMemoryStream(MemoryStream memoryStream)
        {
            IRandomAccessStream randomAccessStream = memoryStream.AsRandomAccessStream();

            Bitmap bitmap = CreateAsync(randomAccessStream).Result;

            return bitmap;
        }

        private static async Task<Bitmap> CreateAsync(IRandomAccessStream inMemoryRandomAccessStream)
        {
            Bitmap bitmap = null;
            BitmapDecoder bitmapDecoder = await BitmapDecoder.CreateAsync(inMemoryRandomAccessStream);
            PixelDataProvider pixelDataAsync = await bitmapDecoder.GetPixelDataAsync();
            int orientedPixelWidth = (int)bitmapDecoder.OrientedPixelWidth;
            int orientedPixelHeight = (int)bitmapDecoder.OrientedPixelHeight;
            byte[] numArray = pixelDataAsync.DetachPixelData();
            bitmap = new Bitmap(orientedPixelWidth, orientedPixelHeight, PixelFormat.Format32bppArgb);
            BitmapData bitmapDatum = bitmap.LockBits(new Rectangle(0, 0, orientedPixelWidth, orientedPixelHeight), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            Marshal.Copy(numArray, 0, bitmapDatum.Scan0, (int)numArray.Length);
            bitmap.UnlockBits(bitmapDatum);
            bitmapDecoder = null;
            return bitmap;
        }

        public void Save(string filename, ImageFormat format)
        {
            FileStream fs = new FileStream(filename, FileMode.Create);
            Save(fs, format);
        }

        public override void Save(Stream stream, ImageFormat format)
        {
            Guid bmpEncoderId = GetBitmapEncoderId(format);

            using (InMemoryRandomAccessStream inMemoryRandomAccessStream = new InMemoryRandomAccessStream())
            {
                Save(inMemoryRandomAccessStream, bmpEncoderId).Wait();
                inMemoryRandomAccessStream.AsStreamForWrite().CopyTo(stream);
            }
        }

        private async Task Save(IRandomAccessStream inMemoryRandomAccessStream, Guid encoderId)
        {
            BitmapEncoder bitmapEncoder = await BitmapEncoder.CreateAsync(encoderId, inMemoryRandomAccessStream);
            Bitmap bitmap = this.Clone(PixelFormat.Format32bppArgb);
            int num = bitmap._height * bitmap._stride;
            byte[] numArray = new byte[num];
            Marshal.Copy(bitmap._scan0, numArray, 0, num);
            bitmapEncoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Straight, (uint)bitmap._width, (uint)bitmap._height, 96, 96, numArray);
            await bitmapEncoder.FlushAsync();
            inMemoryRandomAccessStream.Seek((ulong)0);
        }

        private Guid GetBitmapEncoderId(ImageFormat format)
        {
            Guid bmpEncoderId;
            if (format.Equals(ImageFormat.Bmp))
            {
                bmpEncoderId = BitmapEncoder.BmpEncoderId;
            }
            else if (format.Equals(ImageFormat.Gif))
            {
                bmpEncoderId = BitmapEncoder.GifEncoderId;
            }
            else if (format.Equals(ImageFormat.Jpeg))
            {
                bmpEncoderId = BitmapEncoder.JpegEncoderId;
            }
            else if (format.Equals(ImageFormat.Png))
            {
                bmpEncoderId = BitmapEncoder.PngEncoderId;
            }
            else if (format.Equals(ImageFormat.Tiff))
            {
                bmpEncoderId = BitmapEncoder.TiffEncoderId;
            }
            else
            {
                throw new ArgumentOutOfRangeException("format", format, "Unsupported bitmap encoding format");
            }
            return bmpEncoderId;
        }

        public override void Save(string filename, ImageCodecInfo encoder, EncoderParameters encoderParams)
        {
            throw new NotImplementedException("PCL");
        }

        #endregion

        //internal byte[] Bytes { get { return thi} }
    }
} 