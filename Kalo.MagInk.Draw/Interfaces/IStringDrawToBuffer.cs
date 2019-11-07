using System;

namespace Kalo.MagInk.Draw
{
    public interface IStringDrawToBuffer
    {
        /** Draw a string with specific font, font size and coordinates in the buffer.  */
        void DrawStringAt(byte[] frameBuffer, int x, int y, string text, string fontName, int fontSize, bool colored);
    }
}