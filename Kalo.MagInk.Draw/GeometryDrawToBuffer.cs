using System;

namespace Kalo.MagInk.Draw
{
    public class GeometryDrawToBuffer : IGeometryDrawToBuffer
    {
        private readonly IDrawToBuffer _pixelActions;
        private readonly DrawingArea _drawingArea;

        public GeometryDrawToBuffer(IDrawToBuffer pixelActions, DrawingArea drawingArea)
        {
            _pixelActions = pixelActions;
            _drawingArea = drawingArea;
        }

        public void DrawLine(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored)
        {
            // Bresenham algorithm
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while ((x0 != x1) && (y0 != y1))
            {
                _pixelActions.SetPixel(frameBuffer, x0, y0, colored);
                if (2 * err >= dy)
                {
                    err += dy;
                    x0 += sx;
                }
                if (2 * err <= dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public void DrawHorizontalLine(byte[] frameBuffer, int x, int y, int width, bool colored)
        {
            for (int i = x; i < x + width; i++)
            {
                _pixelActions.SetPixel(frameBuffer, i, y, colored);
            }
        }

        public void DrawVerticalLine(byte[] frameBuffer, int x, int y, int height, bool colored)
        {
            for (int i = y; i < y + height; i++)
            {
                _pixelActions.SetPixel(frameBuffer, x, i, colored);
            }
        }

        public void DrawRectangle(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored)
        {
            int min_x = x1 > x0 ? x0 : x1;
            int max_x = x1 > x0 ? x1 : x0;
            int min_y = y1 > y0 ? y0 : y1;
            int max_y = y1 > y0 ? y1 : y0;
            DrawHorizontalLine(frameBuffer, min_x, min_y, max_x - min_x + 1, colored);
            DrawHorizontalLine(frameBuffer, min_x, max_y, max_x - min_x + 1, colored);
            DrawVerticalLine(frameBuffer, min_x, min_y, max_y - min_y + 1, colored);
            DrawVerticalLine(frameBuffer, max_x, min_y, max_y - min_y + 1, colored);
        }

        public void DrawFilledRectangle(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored)
        {
            int min_x = x1 > x0 ? x0 : x1;
            int max_x = x1 > x0 ? x1 : x0;
            int min_y = y1 > y0 ? x0 : y1;
            int max_y = y1 > y0 ? y1 : y0;
            for (int i = min_x; i < max_x + 1; i++)
            {
                DrawVerticalLine(frameBuffer, i, min_y, max_y - min_y + 1, colored);
            }
        }

        public void DrawCircle(byte[] frameBuffer, int x, int y, int radius, bool colored)
        {
            // Bresenham algorithm
            int x_pos = -radius;
            int y_pos = 0;
            int err = 2 - 2 * radius;

            if (x >= _drawingArea.CurrentWidth || y >= _drawingArea.CurrentHeight)
            {
                return;
            }

            while (true)
            {
                _pixelActions.SetPixel(frameBuffer, x - x_pos, y + y_pos, colored);
                _pixelActions.SetPixel(frameBuffer, x + x_pos, y + y_pos, colored);
                _pixelActions.SetPixel(frameBuffer, x + x_pos, y - y_pos, colored);
                _pixelActions.SetPixel(frameBuffer, x - x_pos, y - y_pos, colored);
                int e2 = err;

                if (e2 <= y_pos)
                {
                    y_pos += 1;
                    err += y_pos * 2 + 1;
                    if (-x_pos == y_pos && e2 <= x_pos)
                    {
                        e2 = 0;
                    }
                }
                if (e2 > x_pos)
                {
                    x_pos += 1;
                    err += x_pos * 2 + 1;
                }
                if (x_pos > 0)
                {
                    break;
                }
            }
        }

        public void DrawFilledCircle(byte[] frameBuffer, int x, int y, int radius, bool colored)
        {
            // Bresenham algorithm
            int x_pos = -radius;
            int y_pos = 0;
            int err = 2 - 2 * radius;

            if (x >= _drawingArea.CurrentWidth || y >= _drawingArea.CurrentHeight)
            {
                return;
            }

            while (true)
            {
                _pixelActions.SetPixel(frameBuffer, x - x_pos, y + y_pos, colored);
                _pixelActions.SetPixel(frameBuffer, x + x_pos, y + y_pos, colored);
                _pixelActions.SetPixel(frameBuffer, x + x_pos, y - y_pos, colored);
                _pixelActions.SetPixel(frameBuffer, x - x_pos, y - y_pos, colored);
                DrawHorizontalLine(frameBuffer, x + x_pos, y + y_pos, 2 * (-x_pos) + 1, colored);
                DrawHorizontalLine(frameBuffer, x + x_pos, y - y_pos, 2 * (-x_pos) + 1, colored);

                int e2 = err;
                if (e2 <= y_pos)
                {
                    y_pos += 1;
                    err += y_pos * 2 + 1;
                    if (-x_pos == y_pos && e2 <= x_pos)
                    {
                        e2 = 0;
                    }
                }
                if (e2 > x_pos)
                {
                    x_pos += 1;
                    err += x_pos * 2 + 1;
                }
                if (x_pos > 0)
                {
                    break;
                }
            }
        }
    }
}