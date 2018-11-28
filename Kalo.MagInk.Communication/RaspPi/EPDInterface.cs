using System.Threading;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace Kalo.MagInk.RaspPi
{
    /**
    EPD hardware interface implements (GPIO, SPI)
    */
    public class EPDInterface {
        
        // Pin definition
        public static readonly int RST_PIN = 17;
        public static readonly int DC_PIN = 25;
        public static readonly int CS_PIN = 8;
        public static readonly int BUSY_PIN = 24;
        // ---------------

        public EPDInterface()
        {
            // Init GPIO
            //GPIO.setmode(GPIO.BCM);
            //GPIO.setwarnings(False);
            Pi.Gpio[RST_PIN].PinMode = GpioPinDriveMode.Output;
            Pi.Gpio[DC_PIN].PinMode = GpioPinDriveMode.Output;
            Pi.Gpio[CS_PIN].PinMode = GpioPinDriveMode.Output;
            Pi.Gpio[BUSY_PIN].PinMode = GpioPinDriveMode.Input;

            // Init SPI
            // SPI device, bus = 0, device = 0
            //SPI = spidev.SpiDev(0, 0)
            // SPI.max_speed_hz = 2000000;
            // SPI.mode = 0b00;
            Pi.Spi.Channel0Frequency = 2000000;
        }

        public void DigitalWrite(int pin, GpioPinValue value)
        {
            Pi.Gpio[pin].Write(value);
        }

        public GpioPinValue DigitalRead(int pin)
        {
            return Pi.Gpio[pin].ReadValue();
        }

        public void DelayMs(int delaytime)
        {
            Thread.Sleep(delaytime);
        }

        public void SPITransfer(byte[] data)
        {
            Pi.Spi.Channel0.SendReceive(data);
            // OR ? Pi.Spi.Channel0.Write(data);
        }
    }
}