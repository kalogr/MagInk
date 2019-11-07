using System;

namespace Kalo.MagInk.Draw
{
    public interface IGeometryDrawToBuffer
    {
        /** Draw a line with start and end coordinates. */
        void DrawLine(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored);

        /** Draw an horizontal line with start coordinates. */
        void DrawHorizontalLine(byte[] frameBuffer, int x, int y, int width, bool colored);

        /** Draw a vertical line with start coordinates. */
        void DrawVerticalLine(byte[] frameBuffer, int x, int y, int height, bool colored);
        
        /** Draw a rectangle with coordinates of angles. */
        void DrawRectangle(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored);

        /** Draw a filled rectangle with coordinates of angles. */
        void DrawFilledRectangle(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored);

        /** Draw a circle with coordinates center and radius. */
        void DrawCircle(byte[] frameBuffer, int x, int y, int radius, bool colored);
           
        /** Draw a filled circle with coordinates center and radius. */
        void DrawFilledCircle(byte[] frameBuffer, int x, int y, int radius, bool colored);
    }
}