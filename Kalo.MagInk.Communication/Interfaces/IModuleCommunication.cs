

using Unosquare.RaspberryIO.Gpio;

namespace Kalo.MagInk.Interfaces
{
    public interface IModuleCommunication
    {
        void Init();

        void DigitalWrite(int pin, GpioPinValue value);

        GpioPinValue DigitalRead(int pin);

        void DelayMs(int delaytime);

        void SendCommand(byte command);

        void SendData(byte data);

        void WaitUntilIdle();

        void Reset();

        void DisplayFrame(byte[] frameBufferBlack, byte[] frameBufferRed) ;

        /**
        * After this, call epd.init() to awaken the module
        */
        void Sleep();
    }
}