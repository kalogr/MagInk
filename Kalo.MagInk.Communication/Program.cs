using System;
using System.Linq;
using Kalo.MagInk.RaspPi;
using NLog;

namespace Kalo.MagInk
{
    class Program
    {

        static void Main(string[] args)
        {
            var logger = LogManager.GetCurrentClassLogger();
            try
            {
                var epd = new EPD();
                epd.Init();

                // Clear the frame buffer
                byte[] frameBlack = Enumerable.Repeat<byte>(0xFF, epd.Width * epd.Height / 8).ToArray();
                byte[] frameRed = Enumerable.Repeat<byte>(0xFF, epd.Width * epd.Height / 8).ToArray();

                // For simplicity, the arguments are explicit numerical coordinates
                epd.DrawRectangle(frameBlack, 10, 80, 50, 140, true);
                epd.DrawLine(frameBlack, 10, 80, 50, 140, true);
                epd.DrawLine(frameBlack, 50, 80, 10, 140, true);
                epd.DrawCircle(frameBlack, 90, 110, 30, true);
                epd.DrawFilledRectangle(frameRed, 10, 180, 50, 240, true);
                epd.DrawFilledRectangle(frameRed, 0, 6, 128, 26, true);
                epd.DrawFilledCircle(frameRed, 90, 210, 30, true);

                // Display the frames
                epd.DisplayFrame(frameBlack, frameRed);

                // # write strings to the buffer
                // font = ImageFont.truetype('/usr/share/fonts/truetype/freefont/FreeMono.ttf', 16)
                // epd.draw_string_at(frame_black, 4, 30, "e-Paper Demo", font, COLORED)
                // epd.draw_string_at(frame_red, 6, 10, "Hello world!", font, UNCOLORED)
                // # display the frames
                // epd.display_frame(frame_black, frame_red)

                // # display images
                // frame_black = epd.get_frame_buffer(Image.open('black.bmp'))
                // frame_red = epd.get_frame_buffer(Image.open('red.bmp'))
                // epd.display_frame(frame_black, frame_red)

            }
            catch (Exception ex)
            {
                // NLog: catch any exception and log it.
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
    }
}
