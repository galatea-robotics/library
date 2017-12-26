﻿/*
 *  This file is adapted from Code Project article "Image Per Pixel Enumeration, Pixel Format Conversion and More"
 *  Copyright (c) 2010-2012 Smart K8
 *  http://www.codeproject.com/Articles/66550/Image-Per-Pixel-Enumeration-Pixel-Format-Conversio
 *  
 *  Released under Code Project Open License, CPOL, http://www.codeproject.com/info/cpol10.aspx
 */

using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace ImagePixelEnumerator.Helpers.Pixels.NonIndexed
{
    /// <summary>
    /// Name |     Blue     |      Green      |     Red      | 
    /// Bit  |00|01|02|03|04|05|06|07|08|09|10|11|12|13|14|15|
    /// Byte |00000000000000000000000|11111111111111111111111|
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 2)]
    internal struct PixelDataRgb565 : INonIndexedPixel
    {
        // raw component values
        [FieldOffset(0)] private readonly Byte blue;     // 00 - 04
        [FieldOffset(0)] private readonly UInt16 green;  // 05 - 10
        [FieldOffset(1)] private readonly Byte red;      // 11 - 15

        // raw high-level values
        [FieldOffset(0)] private UInt16 raw;    // 00 - 15

        // processed component values
        public Int32 Alpha { get { return 0xFF; } }
        public Int32 Red { get { return (red >> 3) & 0xF; } }
        public Int32 Green { get { return (green >> 5) & 0x1F; } }
        public Int32 Blue { get { return blue & 0xF; } }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Argb"/> for more details.
        /// </summary>
        public Int32 Argb
        {
            get { return Pixel.AlphaMask | raw; }
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.GetColor"/> for more details.
        /// </summary>
        public Color GetColor()
        {
            return Color.FromArgb(Argb);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.SetColor"/> for more details.
        /// </summary>
        public void SetColor(Color color)
        {
            color = QuantizationHelper.ConvertAlpha(color);
            raw = (UInt16) ((color.R >> 3) << 11 | (color.G >> 2) << 5 | color.B >> 3);
        }

        /// <summary>
        /// See <see cref="INonIndexedPixel.Value"/> for more details.
        /// </summary>
        public UInt64 Value
        {
            get { return raw; }
            set { raw = (UInt16) (value & 0xFFFF); }
        }
    }
}
