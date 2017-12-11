﻿/*
 *  This file is adapted from Code Project article "Image Per Pixel Enumeration, Pixel Format Conversion and More"
 *  Copyright (c) 2010-2012 Smart K8
 *  http://www.codeproject.com/Articles/66550/Image-Per-Pixel-Enumeration-Pixel-Format-Conversio
 *  
 *  Released under Code Project Open License, CPOL, http://www.codeproject.com/info/cpol10.aspx
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ImagePixelEnumerator.ColorCaches.Common;
using ImagePixelEnumerator.Helpers;

namespace ImagePixelEnumerator.ColorCaches.Octree
{
    internal class OctreeColorCache : BaseColorCache
    {
        #region | Fields |

        private OctreeCacheNode root;

        #endregion

        #region | Properties |

        /// <summary>
        /// Gets a value indicating whether this instance is color model supported.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is color model supported; otherwise, <c>false</c>.
        /// </value>
        public override Boolean IsColorModelSupported
        {
            get { return false; }
        }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Initializes a new instance of the <see cref="OctreeColorCache"/> class.
        /// </summary>
        public OctreeColorCache()
        {
            ColorModel = ColorModel.RedGreenBlue;
            root = new OctreeCacheNode();
        }

        #endregion

        #region << BaseColorCache >>

        /// <summary>
        /// See <see cref="BaseColorCache.Prepare"/> for more details.
        /// </summary>
        public override void Prepare()
        {
            base.Prepare();
            root = new OctreeCacheNode();
        }

        /// <summary>
        /// See <see cref="BaseColorCache.OnCachePalette"/> for more details.
        /// </summary>
        protected override void OnCachePalette(IList<Color> palette)
        {
            Int32 index = 0;

            foreach (Color color in palette)
            {
                root.AddColor(color, index++, 0);
            }
        }

        /// <summary>
        /// See <see cref="BaseColorCache.OnGetColorPaletteIndex"/> for more details.
        /// </summary>
        protected override void OnGetColorPaletteIndex(Color color, out Int32 paletteIndex)
        {
            Dictionary<Int32, Color> candidates = root.GetPaletteIndex(color, 0);

            paletteIndex = 0;
            Int32 index = 0;
            Int32 colorIndex = ColorModelHelper.GetEuclideanDistance(color, ColorModel, candidates.Values.ToList());

            foreach (Int32 colorPaletteIndex in candidates.Keys)
            {
                if (index == colorIndex)
                {
                    paletteIndex = colorPaletteIndex;
                    break;
                }

                index++;
            }
        }

        #endregion
    }
}
