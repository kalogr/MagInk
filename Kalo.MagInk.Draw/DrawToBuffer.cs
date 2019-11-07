using System;
using System.Drawing;

namespace Kalo.MagInk.Draw
{
    /** Set pixel state in a buffer. */
    public class DrawToBuffer : IDrawToBuffer
    {
        private readonly DrawingArea _drawingArea;

        public DrawToBuffer(DrawingArea drawingArea) => _drawingArea = drawingArea;


        /** Set pixel state with rotate parameter.  */
        public void SetPixel(byte[] frameBuffer, int x, int y, bool colored)
        {
            if (x < 0 || x >= _drawingArea.CurrentWidth || y < 0 || y >= _drawingArea.CurrentHeight)
            {
                return;
            }

            if (_drawingArea.CurrentRotate == DrawingArea.ROTATE_0)
            {
                SetAbsolutePixel(frameBuffer, x, y, colored);

            }
            else if (_drawingArea.CurrentRotate == DrawingArea.ROTATE_90)
            {
                int point_temp = x;
                x = _drawingArea.EpdWidth - y;
                y = point_temp;
                SetAbsolutePixel(frameBuffer, x, y, colored);

            }
            else if (_drawingArea.CurrentRotate == DrawingArea.ROTATE_180)
            {
                x = _drawingArea.EpdWidth - x;
                y = _drawingArea.EpdHeight - y;
                SetAbsolutePixel(frameBuffer, x, y, colored);

            }
            else if (_drawingArea.CurrentRotate == DrawingArea.ROTATE_270)
            {
                int point_temp = x;
                x = y;
                y = _drawingArea.EpdHeight - point_temp;
                SetAbsolutePixel(frameBuffer, x, y, colored);
            }
        }

        /** Set pixel state. */
        public void SetAbsolutePixel(byte[] frameBuffer, int x, int y, bool colored)
        {
            // To avoid display orientation effects
            // use EPD_WIDTH instead of self.width
            // use EPD_HEIGHT instead of self.height
            if (x < 0 || x >= _drawingArea.EpdWidth || y < 0 || y >= _drawingArea.EpdHeight)
            {
                return;
            }

            if (colored)
            {
                frameBuffer[(x + y * _drawingArea.EpdWidth) / 8] &= unchecked((byte)(~(0x80 >> (x % 8))));
            }
            else
            {
                frameBuffer[(x + y * _drawingArea.EpdWidth) / 8] |= unchecked((byte)(0x80 >> (x % 8)));
            }
        }

        /** Draw a bitmap (black and white) to the frame buffer at (x,y) position. */
        public void DrawBitmapBw(byte[] frameBuffer, Bitmap image, int x, int y, bool colored)
        {
            if (image == null || frameBuffer == null) { return; }

            int xOrigin = x;
            for (int yImg = 0; yImg < image.Height; yImg++)
            {
                x = xOrigin;
                for (int xImg = 0; xImg < image.Width; xImg++)
                {
                    // Set the bits for the column of pixels at the current position.
                    var pixColor = image.GetPixel(xImg, yImg);
                    if (pixColor.R != 255 && pixColor.G != 255 && pixColor.B != 255)
                    {
                        SetPixel(frameBuffer, x, y, colored);
                    }
                    x++;
                }
                y++;
            }
        }
    }
}