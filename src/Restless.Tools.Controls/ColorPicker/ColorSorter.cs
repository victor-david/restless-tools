﻿using System;
using System.Collections;

namespace Restless.Tools.Controls
{
    internal class ColorSorter : IComparer
    {
        public int Compare(object firstItem, object secondItem)
        {
            if (firstItem == null || secondItem == null)
                return -1;

            ColorItem colorItem1 = (ColorItem)firstItem;
            ColorItem colorItem2 = (ColorItem)secondItem;

            if (colorItem1.Color == null || colorItem2.Color == null)
            {
                return -1;
            }

            System.Drawing.Color drawingColor1 = System.Drawing.Color.FromArgb(colorItem1.Color.A, colorItem1.Color.R, colorItem1.Color.G, colorItem1.Color.B);
            System.Drawing.Color drawingColor2 = System.Drawing.Color.FromArgb(colorItem2.Color.A, colorItem2.Color.R, colorItem2.Color.G, colorItem2.Color.B);

            // Compare Hue
            double hueColor1 = Math.Round((double)drawingColor1.GetHue(), 3);
            double hueColor2 = Math.Round((double)drawingColor2.GetHue(), 3);

            if (hueColor1 > hueColor2)
                return 1;
            else if (hueColor1 < hueColor2)
                return -1;
            else
            {
                // Hue is equal, compare Saturation
                double satColor1 = Math.Round((double)drawingColor1.GetSaturation(), 3);
                double satColor2 = Math.Round((double)drawingColor2.GetSaturation(), 3);

                if (satColor1 > satColor2)
                    return 1;
                else if (satColor1 < satColor2)
                    return -1;
                else
                {
                    // Saturation is equal, compare Brightness
                    double brightColor1 = Math.Round((double)drawingColor1.GetBrightness(), 3);
                    double brightColor2 = Math.Round((double)drawingColor2.GetBrightness(), 3);

                    if (brightColor1 > brightColor2)
                        return 1;
                    else if (brightColor1 < brightColor2)
                        return -1;
                }
            }

            return 0;
        }
    }
}
