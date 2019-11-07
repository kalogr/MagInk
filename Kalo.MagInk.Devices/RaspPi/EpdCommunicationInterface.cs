using System.Threading;
using System.Device.Gpio.Drivers;
using System.Device.Gpio;
using System.Device.Spi;
using Iot.Device.Spi;
using System;
using NLog;

namespace Kalo.MagInk.Devices.RaspPi
{
    /** Electronic paper display hardware interface implementation (with GPIO pin in synchronous serial communication [SPI]). */
    public class EpdCommunicationInterface : IDisposable
    {
        // Pin definition
        public static readonly int RST_PIN = 11;
        public static readonly int DC_PIN = 22;
        public static readonly int CS_PIN = 24; // CE0
        public static readonly int BUSY_PIN = 18;
        public static readonly int CLK_PIN = 23; // SCLK
        public static readonly int MOSI_PIN = 19; // DIN
        public static readonly int MISO_PIN = 21;
        // ---------------

        /** Flag: Has Dispose already been called? */
        private bool disposed = false;

        /** Access to GPIO pins. */
        private GpioController controller;

        /** SPI device. */
        private SpiDevice device;

        /** Logger. */
        private Logger logger = NLog.LogManager.GetCurrentClassLogger();


        public EpdCommunicationInterface()
        {
            logger.Info("Create EPDCommunicationInterface");
            // Init GPIO //
            //GPIO.setmode(GPIO.BCM);
            //GPIO.setwarnings(False);
            // Pi.Gpio[RST_PIN].PinMode = GpioPinDriveMode.Output;
            // Pi.Gpio[DC_PIN].PinMode = GpioPinDriveMode.Output;
            // Pi.Gpio[CS_PIN].PinMode = GpioPinDriveMode.Output;
            // Pi.Gpio[BUSY_PIN].PinMode = GpioPinDriveMode.Input;
            var driver = new RaspberryPi3Driver();
            controller = new GpioController(PinNumberingScheme.Board, driver);

            // Init SPI //
            // SPI device, bus = 0, device = 0
            // SPI = spidev.SpiDev(0, 0)
            // SPI.max_speed_hz = 2000000;
            // SPI.mode = 0b00;
            // Pi.Spi.Channel0Frequency = 2000000;
            var busId = 0;
            var chipSelectLine = 0;
            var settings = new SpiConnectionSettings(busId, chipSelectLine)
            {
                // Mode = SpiMode.Mode0,
                ClockFrequency = 2000000
            };

            device = new SoftwareSpi(clk: CLK_PIN, miso: MISO_PIN, mosi: MOSI_PIN, cs: CS_PIN, settings, controller);
        }

        /** Write value to a pin. */
        public void DigitalWrite(int pin, PinValue value)
        {
            if (!controller.IsPinOpen(pin))
            { controller.OpenPin(pin, PinMode.Output); }

            controller.Write(pin, value);
            //controller.ClosePin(pin);
        }

        /** Read value to a pin. */
        public PinValue DigitalRead(int pin)
        {
            if (!controller.IsPinOpen(pin))
            { controller.OpenPin(pin, PinMode.Input); }
            // controller.ClosePin(pin);

            return controller.Read(pin);
        }

        /** Delay to sleep. */
        public void DelayMs(int delaytime)
        {
            Thread.Sleep(delaytime);
        }

        /** Transfer data with SPI. */
        public void SPITransfer(byte[] data)
        {
            device.Write(data);
        }

        #region Dispose
        public void Dispose()
        {
            Dispose(true);
        }

        /** Protected implementation of Dispose pattern. */
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                controller.Dispose();
                device.Dispose();
            }

            disposed = true;
        }
        #endregion
    }
}