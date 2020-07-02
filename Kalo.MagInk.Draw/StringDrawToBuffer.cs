using System;
using System.Drawing;
using Kalo.MagInk.Draw.Interface;
using NLog;

namespace Kalo.MagInk.Draw
{
    public class StringDrawToBuffer : IStringDrawToBuffer
    {
        private readonly IDrawToBuffer _pixelActions;
        private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();


        public StringDrawToBuffer(IDrawToBuffer pixelActions) => _pixelActions = pixelActions;


        /** Draw a string with specific font, font size and coordinates in the buffer.  */
        public void DrawStringAt(byte[] frameBuffer, int x, int y, string text, string fontName, int fontSize, bool colored)
        {
            // Create image with text
            Bitmap image = ConvertTextToImage(text, fontName, fontSize, 100, 30);

            // Navigate on all pixels image
            _pixelActions.DrawBitmapBw(frameBuffer, image, x, y, colored);
        }

        /** Convert a text to an bitmap image. */
        private Bitmap ConvertTextToImage(string text, string fontName, int fontSize, int width, int height)
        {
            try
            {
                Bitmap bmp = new Bitmap(width, height);
                using Graphics graphics = Graphics.FromImage(bmp);
                Font font = new Font(fontName, fontSize);

                graphics.FillRectangle(new SolidBrush(Color.White), 0, 0, bmp.Width, bmp.Height);
                graphics.DrawString(text, font, new SolidBrush(Color.Black), 0, 0);
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();

                return bmp;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Convert text to image >");
                return null;
            }
        }
    }
}