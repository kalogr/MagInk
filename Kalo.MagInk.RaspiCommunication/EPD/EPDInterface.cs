/*
EPD hardware interface implements (GPIO, SPI)
 */
public class EPDInterface {
    
    // Pin definition
    public static readonly int RST_PIN = 17;
    public static readonly int DC_PIN = 25;
    public static readonly int CS_PIN = 8;
    public static readonly int BUSY_PIN = 24;

    //private SPI SPI;

    public EPDInterface()
    {
        // SPI device, bus = 0, device = 0
        //SPI = spidev.SpiDev(0, 0)

        // Initi GPIO/SPI
        // GPIO.setmode(GPIO.BCM);
        // GPIO.setwarnings(False);
        // GPIO.setup(RST_PIN, GPIO.OUT);
        // GPIO.setup(DC_PIN, GPIO.OUT);
        // GPIO.setup(CS_PIN, GPIO.OUT);
        // GPIO.setup(BUSY_PIN, GPIO.IN);
        // SPI.max_speed_hz = 2000000;
        // SPI.mode = 0b00;
    }

    public void EPDDigitalWrite(int pin, ??? value)
    {
        //GPIO.output(pin, value);
    }

    public ??? EPDDigitalRead(int pin)
    {
        //return GPIO.input(BUSY_PIN);
    }

    public void EPDDelayMs(int delaytime)
    {
        //time.sleep(delaytime / 1000.0);
    }

    public void SPITransfer(??? data)
    {
        //SPI.writebytes(data);
    }


}