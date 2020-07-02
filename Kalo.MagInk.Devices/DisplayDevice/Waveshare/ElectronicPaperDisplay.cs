using System.Device.Gpio;
using Kalo.MagInk.Devices.Interface;
using Kalo.MagInk.Devices.RaspPi;
using Kalo.MagInk.Draw;
using NLog;

namespace Kalo.MagInk.Devices.DisplayDevice.Waveshare
{
    /** Define a generic Electronic Paper Display (EPD). */
    public class ElectronicPaperDisplay : IElectronicPaperDisplay
    {
        #region Properties

        public string Name { get; protected set; }

        public int EPaperDisplayWidth { get; protected set; }
        public int EPaperDisplayHeight { get; protected set; }

        #endregion


        #region Private variables

        private readonly DrawingArea _drawingArea;
        private readonly EpdCommunicationInterface _epdCommInt;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion


        public ElectronicPaperDisplay(DrawingArea drawingArea, int epdWidth, int epdHeight)
        {
            _epdCommInt = new EpdCommunicationInterface();

            EPaperDisplayWidth = epdWidth;
            EPaperDisplayHeight = epdHeight;

            _drawingArea = drawingArea;
            _drawingArea.CurrentWidth = EPaperDisplayWidth;
            _drawingArea.CurrentHeight = EPaperDisplayHeight;
            _drawingArea.CurrentRotate = DrawingArea.ROTATE_0;
            _drawingArea.EpdWidth = EPaperDisplayWidth;
            _drawingArea.EpdHeight = EPaperDisplayHeight;

            _logger.Info($"Create Electronic Paper Display [{EPaperDisplayWidth},{EPaperDisplayHeight}]");
        }

        #region Communication module

        /** Init communication to device. */
        public void Init()
        {
            _logger.Info("Init EPD");
            Reset();
            _logger.Info("BOOSTER_SOFT_START");
            SendCommand(EpdCommands.BOOSTER_SOFT_START);
            SendData(0x17);
            SendData(0x17);
            SendData(0x17);
            _logger.Info("POWER_ON");
            SendCommand(EpdCommands.POWER_ON);
            WaitUntilIdle();
            _logger.Info("PANEL_SETTING");
            SendCommand(EpdCommands.PANEL_SETTING);
            SendData(0x8F);
            _logger.Info("VCOM_AND_DATA_INTERVAL_SETTING");
            SendCommand(EpdCommands.VCOM_AND_DATA_INTERVAL_SETTING);
            SendData(0x77);
            _logger.Info("TCON_RESOLUTION");
            SendCommand(EpdCommands.TCON_RESOLUTION);
            SendData(0x80);
            SendData(0x01);
            SendData(0x28);
            _logger.Info("VCM_DC_SETTING_REGISTER");
            SendCommand(EpdCommands.VCM_DC_SETTING_REGISTER);
            SendData(0x0A);
        }

        /** Write value to a pin. */
        public void DigitalWrite(int pin, PinValue value)
        {
// #if DEBUG
//             logger.Info($"Digital write [pin={pin}, value={value}]");
// #endif

            _epdCommInt.DigitalWrite(pin, value);
        }

        /** Read value to a pin. */
        public PinValue DigitalRead(int pin)
        {
// #if DEBUG
//             logger.Info($"Digital read [pin={pin}]");
// #endif

            return _epdCommInt.DigitalRead(pin);
        }

        /** Delay to sleep. */
        public void DelayMs(int delaytime)
        {
            _epdCommInt.DelayMs(delaytime);
        }

        /** Send command. */
        public void SendCommand(byte command)
        {
            _logger.Info($"Send command [command={command}]");
            DigitalWrite(EpdCommunicationInterface.DC_PIN, PinValue.Low);
            _epdCommInt.SPITransfer(new[] { command });
        }

        /** Send data. */
        public void SendData(byte data)
        {
// #if DEBUG
//             logger.Info($"Send data [data={data}]");
// #endif
            DigitalWrite(EpdCommunicationInterface.DC_PIN, PinValue.High);
            _epdCommInt.SPITransfer(new[] { data });
        }

        /** Wait until idle. */
        public void WaitUntilIdle()
        {
            _logger.Info("Wait until idle.");
            while (DigitalRead(EpdCommunicationInterface.BUSY_PIN) == 0) // 0: busy, 1: idle
            { DelayMs(100); }
        }

        /** Reset device. */
        public void Reset()
        {
            _logger.Info("Reset device.");
            DigitalWrite(EpdCommunicationInterface.RST_PIN, PinValue.Low);
            DelayMs(200);
            DigitalWrite(EpdCommunicationInterface.RST_PIN, PinValue.High);
            DelayMs(200);
        }

        /** Display black and red frame to the device. FrameBuffer to null for ignore it. */
        public void DisplayFrame(byte[] frameBufferBlack, byte[] frameBufferRed)
        {
            _logger.Info("Display frame.");

            if (frameBufferBlack != null)
            {
                SendCommand(EpdCommands.DATA_START_TRANSMISSION_1);
                DelayMs(2);
                for (int i = 0; i < _drawingArea.CurrentWidth * _drawingArea.CurrentHeight / 8; i++)
                {
                    SendData(frameBufferBlack[i]);
                }
                DelayMs(2);
            }

            if (frameBufferRed != null)
            {
                SendCommand(EpdCommands.DATA_START_TRANSMISSION_2);
                DelayMs(2);
                for (int i = 0; i < _drawingArea.CurrentWidth * _drawingArea.CurrentHeight / 8; i++)
                {
                    SendData(frameBufferRed[i]);
                }
                DelayMs(2);
            }

            SendCommand(EpdCommands.DISPLAY_REFRESH);
            WaitUntilIdle();
        }

        /**
         * Device in sleep mode.
         * After this, call epd.init() to awaken the module. */
        public void Sleep()
        {
            _logger.Info("Device in sleep mode.");
            SendCommand(EpdCommands.VCOM_AND_DATA_INTERVAL_SETTING);
            SendData(0x37);
            SendCommand(EpdCommands.VCM_DC_SETTING_REGISTER); // to solve Vcom drop 
            SendData(0x00);
            SendCommand(EpdCommands.POWER_SETTING); // power setting
            SendData(0x02); // gate switch to external
            SendData(0x00);
            SendData(0x00);
            SendData(0x00);
            WaitUntilIdle();
            SendCommand(EpdCommands.POWER_OFF); // power off
        }

        #endregion

        /** Display rotate. */
        public void SetRotate(int rotateValue)
        {
            if (rotateValue == DrawingArea.ROTATE_0)
            {
                _drawingArea.CurrentRotate = DrawingArea.ROTATE_0;
                _drawingArea.CurrentWidth = EPaperDisplayWidth;
                _drawingArea.CurrentHeight = EPaperDisplayHeight;

            }
            else if (rotateValue == DrawingArea.ROTATE_90)
            {
                _drawingArea.CurrentRotate = DrawingArea.ROTATE_90;
                _drawingArea.CurrentWidth = EPaperDisplayHeight;
                _drawingArea.CurrentHeight = EPaperDisplayWidth;

            }
            else if (rotateValue == DrawingArea.ROTATE_180)
            {
                _drawingArea.CurrentRotate = DrawingArea.ROTATE_180;
                _drawingArea.CurrentWidth = EPaperDisplayWidth;
                _drawingArea.CurrentHeight = EPaperDisplayHeight;

            }
            else if (rotateValue == DrawingArea.ROTATE_270)
            {
                _drawingArea.CurrentRotate = DrawingArea.ROTATE_270;
                _drawingArea.CurrentWidth = EPaperDisplayHeight;
                _drawingArea.CurrentHeight = EPaperDisplayWidth;
            }
        }

    }
}