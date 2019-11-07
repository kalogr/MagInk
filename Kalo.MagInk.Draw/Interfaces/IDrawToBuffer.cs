using System;
using System.Drawing;

namespace Kalo.MagInk.Draw
{
    public interface IDrawToBuffer
    {
        /** Set pixel state with rotate parameter.  */
        void SetPixel(byte[] frameBuffer, int x, int y, bool colored);

        /** Set pixel state. */
        void SetAbsolutePixel(byte[] frameBuffer, int x, int y, bool colored);

        /** Draw a bitmap (black and white) to the frame buffer at (x,y) position. */
        void DrawBitmapBw(byte[] frameBuffer, Bitmap image, int x, int y, bool colored);
    }
}