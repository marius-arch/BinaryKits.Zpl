﻿using BarcodeLib;
using BinaryKits.Zpl.Label.Elements;
using BinaryKits.Zpl.Viewer.Helpers;
using SkiaSharp;
using System;
using System.Drawing;

namespace BinaryKits.Zpl.Viewer.ElementDrawers
{
    public class Barcode93ElementDrawer : BarcodeDrawerBase
    {
        ///<inheritdoc/>
        public override bool CanDraw(ZplElementBase element)
        {
            return element is ZplBarcode93;
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element)
        {
            Draw(element, new DrawerOptions());
        }

        ///<inheritdoc/>
        public override void Draw(ZplElementBase element, DrawerOptions options)
        {
            if (element is ZplBarcode93 barcode)
            {
                float x = barcode.PositionX;
                float y = barcode.PositionY;

                var content = barcode.Content;

                float labelFontSize = Math.Min(barcode.ModuleWidth * 7.2f, 72f);
                var labelTypeFace = options.FontLoader("A");
                var labelFont = new SKFont(labelTypeFace, labelFontSize).ToSystemDrawingFont();
                int labelHeight = barcode.PrintInterpretationLine ? labelFont.Height : 0;
                int labelHeightOffset = barcode.PrintInterpretationLineAboveCode ? labelHeight : 0;

                var barcodeElement = new Barcode
                {
                    BarWidth = barcode.ModuleWidth,
                    BackColor = Color.Transparent,
                    Height = barcode.Height + labelHeight,
                    IncludeLabel = barcode.PrintInterpretationLine,
                    LabelPosition = barcode.PrintInterpretationLineAboveCode ? LabelPositions.TOPCENTER : LabelPositions.BOTTOMCENTER,
                    LabelFont = labelFont,
                    AlternateLabel = content
                };

                using var image = barcodeElement.Encode(TYPE.CODE93, content);
                this.DrawBarcode(this.GetImageData(image), barcode.Height, image.Width, barcode.FieldOrigin != null, x, y, labelHeightOffset, barcode.FieldOrientation);
            }
        }
    }
}
