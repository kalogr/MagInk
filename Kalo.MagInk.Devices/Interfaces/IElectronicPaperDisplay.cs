using System.Device.Gpio;

namespace Kalo.MagInk.Devices
{
    public interface IElectronicPaperDisplay
    {
        /** ePaper model name. */
        string Name { get; }

        /** ePaper screen width size. */
        int EPaperDisplayWidth { get; }

        /** ePaper screen height size. */
        int EPaperDisplayHeight { get; }


        /** Init communication to device. */
        void Init();

        /** Write value to a pin. */
        void DigitalWrite(int pin, PinValue value);

        /** Read value to a pin. */
        PinValue DigitalRead(int pin);

        /** Delay to sleep. */
        void DelayMs(int delaytime);

        /** Send command. */
        void SendCommand(byte command);

        /** Send data. */
        void SendData(byte data);

        /** Wait until idle. */
        void WaitUntilIdle();

        /** Reset device. */
        void Reset();

        /** Display black and red frame to the device. */
        void DisplayFrame(byte[] frameBufferBlack, byte[] frameBufferRed);

        /**
         * Device in sleep mode.
         * After this, call epd.init() to awaken the module. */
        void Sleep();
    }
}