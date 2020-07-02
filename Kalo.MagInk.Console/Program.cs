using System;
using System.Linq;
using Kalo.MagInk.Devices;
using Kalo.MagInk.Devices.DisplayDevice.Waveshare;
using Kalo.MagInk.Devices.Interface;
using Kalo.MagInk.Draw;
using Kalo.MagInk.Draw.Interface;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace Kalo.MagInk.Console
{
    class Program
    {
        private static Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static IServiceProvider _serviceProvider;

        static void Main(string[] args)
        {
            RegisterServices();

            try
            {  
                System.Console.WriteLine(">> Start...");
                var epd = _serviceProvider.GetService<IElectronicPaperDisplay>();
                var geometryDrawTools = _serviceProvider.GetService<IGeometryDrawToBuffer>();
                var stringDrawTools = _serviceProvider.GetService<IStringDrawToBuffer>();
                var drawingArea = _serviceProvider.GetService<DrawingArea>();


                System.Console.WriteLine($"> Init EPD {epd.Name} [{drawingArea.CurrentWidth}, {drawingArea.CurrentHeight}]");
                epd.Init();

                System.Console.WriteLine($"> Draw");
                // Clear the frame buffer
                byte[] frameBlack = Enumerable.Repeat<byte>(0xFF, drawingArea.CurrentWidth * drawingArea.CurrentHeight / 8).ToArray();
                byte[] frameRed = Enumerable.Repeat<byte>(0xFF, drawingArea.CurrentWidth * drawingArea.CurrentHeight / 8).ToArray();

                // // For simplicity, the arguments are explicit numerical coordinates
                //geometryDrawTools.DrawRectangle(frameBlack, 10, 80, 50, 140, true);
                //geometryDrawTools.DrawLine(frameBlack, 10, 80, 50, 140, true);
                //geometryDrawTools.DrawLine(frameBlack, 50, 80, 10, 140, true);
                //geometryDrawTools.DrawCircle(frameBlack, 90, 110, 30, true);
                //geometryDrawTools.DrawFilledRectangle(frameRed, 10, 180, 50, 240, true);
                //geometryDrawTools.DrawFilledRectangle(frameRed, 0, 6, 128, 26, true);
                //geometryDrawTools.DrawFilledCircle(frameRed, 90, 210, 30, true);
                geometryDrawTools.DrawFilledRectangle(frameBlack, 5, 5, 55, 55, true);
                geometryDrawTools.DrawFilledRectangle(frameRed, 25, 25, 35, 35, true);


                // Write strings to the buffer
                //stringDrawTools.DrawStringAt(frameBlack, 4, 70, "*Prout*!", "/usr/share/fonts/truetype/freefont/FreeMono.ttf", 16, true);
                //geometryDrawTools.DrawRectangle(frameRed, 3, 69, 105, 100, true);
                //stringDrawTools.DrawStringAt(frameRed, 6, 10, "Hello world !", "/usr/share/fonts/truetype/freefont/FreeMono.ttf", 16, false);

                // Display the frames
                System.Console.WriteLine("> Display frame");
                epd.DisplayFrame(frameBlack, frameRed);

                System.Console.WriteLine("...End <<");
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, "Error in MagInk main program >");
            }

            DisposeServices();
        }

        #region Dependency injection

        /** Registration of services. */
        private static void RegisterServices()
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<DrawingArea>();
            collection.AddSingleton<IDrawToBuffer, DrawToBuffer>();
            collection.AddSingleton<IStringDrawToBuffer, StringDrawToBuffer>();
            collection.AddSingleton<IGeometryDrawToBuffer, GeometryDrawToBuffer>();
            collection.AddSingleton<IElectronicPaperDisplay, ElectronicPaperDisplay2in9B>();

            _serviceProvider = collection.BuildServiceProvider();
        }

        /** Dispose services. */
        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }

            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
        
        #endregion

    }
}
