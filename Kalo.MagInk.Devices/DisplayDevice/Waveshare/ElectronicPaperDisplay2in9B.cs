using Kalo.MagInk.Draw;

namespace Kalo.MagInk.Devices.DisplayDevice.Waveshare
{
    /** Define a 2.9inch e-Paper Module (B) (Electronic Paper Display). */
    public class ElectronicPaperDisplay2in9B : ElectronicPaperDisplay
    {
        /** Default 2.9inch e-Paper Module (B) constructor. */
        public ElectronicPaperDisplay2in9B(DrawingArea drawingArea) : base(drawingArea, 128, 296) 
        { 
            Name = "2.9inch e-Paper Module (B)";
        }
    }
}