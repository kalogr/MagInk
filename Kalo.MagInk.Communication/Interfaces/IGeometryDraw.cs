

namespace Kalo.MagInk.Interfaces
{
    public interface IGeometryDraw
    {
        void DrawLine(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored);

        void DrawHorizontalLine(byte[] frameBuffer, int x, int y, int width, bool colored);

        void DrawVerticalLine(byte[] frameBuffer, int x, int y, int height, bool colored);
        
        void DrawRectangle(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored);

        void DrawFilledRectangle(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored);

        void DrawCircle(byte[] frameBuffer, int x, int y, int radius, bool colored);
           
        void DrawFilledCircle(byte[] frameBuffer, int x, int y, int radius, bool colored);
    }
}