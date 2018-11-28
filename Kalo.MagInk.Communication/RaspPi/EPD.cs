using System;
using System.Linq;
using Kalo.MagInk.Interfaces;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace Kalo.MagInk.RaspPi
{
    public class EPD : IGeometryDraw, IModuleCommunication
    {
        // Display resolution
        public static readonly int EPD_WIDTH = 128;
        public static readonly int EPD_HEIGHT = 296;
        // ------------------

        // Display orientation
        public static readonly int ROTATE_0 = 0;
        public static readonly int ROTATE_90 = 1;
        public static readonly int ROTATE_180 = 2;
        public static readonly int ROTATE_270 = 3;
        // -------------------

        // Properties
        public int Width { get; set; }
        public int Height { get; set; }

        // Locals variables
        private EPDInterface epdInt;
        
        private int reset_pin;
        private int dc_pin;
        private int busy_pin;
        private int rotate;


        public EPD()
        {
            epdInt = new EPDInterface();
            reset_pin = EPDInterface.RST_PIN;
            dc_pin = EPDInterface.DC_PIN;
            busy_pin = EPDInterface.BUSY_PIN;
            Width = EPD_WIDTH;
            Height = EPD_HEIGHT;
            rotate = ROTATE_0;
        }

        #region Module Communication
        public void Init()
        {
            Reset();
            SendCommand(EPD2in9bCommands.BOOSTER_SOFT_START);
            SendData(0x17);
            SendData(0x17);
            SendData(0x17);
            SendCommand(EPD2in9bCommands.POWER_ON);
            WaitUntilIdle();
            SendCommand(EPD2in9bCommands.PANEL_SETTING);
            SendData(0x8F);
            SendCommand(EPD2in9bCommands.VCOM_AND_DATA_INTERVAL_SETTING);
            SendData(0x77);
            SendCommand(EPD2in9bCommands.TCON_RESOLUTION);
            SendData(0x80);
            SendData(0x01);
            SendData(0x28);
            SendCommand(EPD2in9bCommands.VCM_DC_SETTING_REGISTER);
            SendData(0x0A);
        }

        public void DigitalWrite(int pin, GpioPinValue value)
        {
            epdInt.DigitalWrite(pin, value);
        }

        public GpioPinValue DigitalRead(int pin)
        {
            return epdInt.DigitalRead(pin);
        }

        public void DelayMs(int delaytime)
        {
            epdInt.DelayMs(delaytime);
        }

        public void SendCommand(byte command)
        {
            DigitalWrite(EPDInterface.DC_PIN, GpioPinValue.Low);
            epdInt.SPITransfer(new[] { command });
        }

        public void SendData(byte data)
        {
            DigitalWrite(EPDInterface.DC_PIN, GpioPinValue.High);
            epdInt.SPITransfer(new[] { data });
        }

        public void WaitUntilIdle()
        {
            while(DigitalRead(EPDInterface.BUSY_PIN) == 0) // 0: busy, 1: idle
                {DelayMs(100);}
        }

        public void Reset()
        {
            DigitalWrite(EPDInterface.RST_PIN, GpioPinValue.Low); // module reset
            DelayMs(200);
            DigitalWrite(EPDInterface.RST_PIN, GpioPinValue.High);
            DelayMs(200);
        }

        // public byte[] getFrameBuffer(image)
        // {
        //     byte[] buf = Enumerable.Repeat<byte>(0xFF, width * height / 8).ToArray();

        //     // Set buffer to value of Python Imaging Library image.
        //     // Image must be in mode 1.
        //     var image_monocolor = image.convert('1');
        //     int imwidth = image_monocolor.imwidth;
        //     int imheight = image_monocolor.imheight;

        //     if (imwidth != width || imheight != height) {
        //         throw new InvalidOperationException($"Image must be same dimensions as display ({width}x{height}).");
        //     }

        //     var pixels = image_monocolor.load();
        //     for (int y = 0; y < height; y++) {
        //         for (int x = 0; x < width; x++) {
        //             // Set the bits for the column of pixels at the current position.
        //             if (pixels[x, y] == 0) {
        //                 buf[(x + y * width) / 8] &= ~(0x80 >> (x % 8));
        //             }
        //         }
        //     }

        //     return buf;
        // }

        public void DisplayFrame(byte[] frameBufferBlack, byte[] frameBufferRed) 
        {
            if (frameBufferBlack != null) {
                SendCommand(EPD2in9bCommands.DATA_START_TRANSMISSION_1);
                DelayMs(2);
                for (int i = 0; i < Width * Height / 8; i++) {
                    SendData(frameBufferBlack[i]);
                }
                DelayMs(2);
            }

            if (frameBufferRed != null) {
                SendCommand(EPD2in9bCommands.DATA_START_TRANSMISSION_2);
                DelayMs(2);
                for (int i = 0; i < Width * Height / 8; i++) {
                    SendData(frameBufferRed[i]);
                }
                DelayMs(2);
            }

            SendCommand(EPD2in9bCommands.DISPLAY_REFRESH);
            WaitUntilIdle();
        }

        /**
        * After this, call epd.init() to awaken the module
        */
        public void Sleep() {
            SendCommand(EPD2in9bCommands.VCOM_AND_DATA_INTERVAL_SETTING);
            SendData(0x37);
            SendCommand(EPD2in9bCommands.VCM_DC_SETTING_REGISTER); // to solve Vcom drop 
            SendData(0x00);
            SendCommand(EPD2in9bCommands.POWER_SETTING); // power setting
            SendData(0x02); // gate switch to external
            SendData(0x00);
            SendData(0x00);
            SendData(0x00);
            WaitUntilIdle();
            SendCommand(EPD2in9bCommands.POWER_OFF); // power off
        }
        #endregion

        public void SetRotate(int rotateValue) {
            if (rotateValue == ROTATE_0) {
                rotate = ROTATE_0;
                Width = EPD_WIDTH;
                Height = EPD_HEIGHT;

            } else if (rotateValue == ROTATE_90) {
                rotate = ROTATE_90;
                Width = EPD_HEIGHT;
                Height = EPD_WIDTH;

            } else if (rotateValue == ROTATE_180) {
                rotate = ROTATE_180;
                Width = EPD_WIDTH;
                Height = EPD_HEIGHT;

            } else if (rotateValue == ROTATE_270) {
                rotate = ROTATE_270;
                Width = EPD_HEIGHT;
                Height = EPD_WIDTH;
            }
        }

        public void SetPixel(byte[] frameBuffer, int x, int y, bool colored) {
            if (x < 0 || x >= Width || y < 0 || y >= Height) {
                return;
            }

            if (rotate == ROTATE_0) {
                SetAbsolutePixel(frameBuffer, x, y, colored);

            } else if (rotate == ROTATE_90) {
                int point_temp = x;
                x = EPD_WIDTH - y;
                y = point_temp;
                SetAbsolutePixel(frameBuffer, x, y, colored);

            } else if (rotate == ROTATE_180) {
                x = EPD_WIDTH - x;
                y = EPD_HEIGHT- y;
                SetAbsolutePixel(frameBuffer, x, y, colored);

            } else if (rotate == ROTATE_270) {
                int point_temp = x;
                x = y;
                y = EPD_HEIGHT - point_temp;
                SetAbsolutePixel(frameBuffer, x, y, colored);
            }
        }

        public void SetAbsolutePixel(byte[] frameBuffer, int x, int y, bool colored) {
            // To avoid display orientation effects
            // use EPD_WIDTH instead of self.width
            // use EPD_HEIGHT instead of self.height
            if (x < 0 || x >= EPD_WIDTH || y < 0 || y >= EPD_HEIGHT) {
                return;
            }

            if (colored){
                frameBuffer[(x + y * EPD_WIDTH) / 8] &= unchecked((byte)(~(0x80 >> (x % 8))));
            } else {
                frameBuffer[(x + y * EPD_WIDTH) / 8] |= unchecked((byte)(0x80 >> (x % 8)));
            }
        }

        #region GEOMETRY DRAW
        public void DrawLine(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored) {
            // Bresenham algorithm
            int dx = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int dy = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while((x0 != x1) && (y0 != y1)) {
                SetPixel(frameBuffer, x0, y0 , colored);
                if (2 * err >= dy) {
                    err += dy;
                    x0 += sx;
                }
                if (2 * err <= dx) {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        public void DrawHorizontalLine(byte[] frameBuffer, int x, int y, int width, bool colored) {
            for (int i = x; i < x + width; i++) {
                SetPixel(frameBuffer, i, y, colored);
            }
        }

        public void DrawVerticalLine(byte[] frameBuffer, int x, int y, int height, bool colored) {
            for (int i = y; i < y + height; i++) {
                SetPixel(frameBuffer, x, i, colored);
            }
        }

        public void DrawRectangle(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored) {
            int min_x = x1 > x0 ? x0 : x1;
            int max_x = x1 > x0 ? x1 : x0;
            int min_y = y1 > y0 ? y0 : y1;
            int max_y = y1 > y0 ? y1 : y0;
            DrawHorizontalLine(frameBuffer, min_x, min_y, max_x - min_x + 1, colored);
            DrawHorizontalLine(frameBuffer, min_x, max_y, max_x - min_x + 1, colored);
            DrawVerticalLine(frameBuffer, min_x, min_y, max_y - min_y + 1, colored);
            DrawVerticalLine(frameBuffer, max_x, min_y, max_y - min_y + 1, colored);
        }

        public void DrawFilledRectangle(byte[] frameBuffer, int x0, int y0, int x1, int y1, bool colored) {
            int min_x = x1 > x0 ? x0 : x1;
            int max_x = x1 > x0 ? x1 : x0;
            int min_y = y1 > y0 ? x0 : y1;
            int max_y = y1 > y0 ? y1 : y0;
            for (int i = min_x; i < max_x + 1; i++) {
                DrawVerticalLine(frameBuffer, i, min_y, max_y - min_y + 1, colored);
            }
        }

        public void DrawCircle(byte[] frameBuffer, int x, int y, int radius, bool colored) {
            // Bresenham algorithm
            int x_pos = -radius;
            int y_pos = 0;
            int err = 2 - 2 * radius;

            if (x >= Width || y >= Height) {
                return;
            }

            while(true) {
                SetPixel(frameBuffer, x - x_pos, y + y_pos, colored);
                SetPixel(frameBuffer, x + x_pos, y + y_pos, colored);
                SetPixel(frameBuffer, x + x_pos, y - y_pos, colored);
                SetPixel(frameBuffer, x - x_pos, y - y_pos, colored);
                int e2 = err;

                if (e2 <= y_pos) {
                    y_pos += 1;
                    err += y_pos * 2 + 1;
                    if(-x_pos == y_pos && e2 <= x_pos) {
                        e2 = 0;
                    }
                }
                if (e2 > x_pos) {
                    x_pos += 1;
                    err += x_pos * 2 + 1;
                }
                if (x_pos > 0) {
                    break;
                }
            }
        }

        public void DrawFilledCircle(byte[] frameBuffer, int x, int y, int radius, bool colored) {
            // Bresenham algorithm
            int x_pos = -radius;
            int y_pos = 0;
            int err = 2 - 2 * radius;

            if (x >= Width || y >= Height) {
                return;
            }

            while (true) {
                SetPixel(frameBuffer, x - x_pos, y + y_pos, colored);
                SetPixel(frameBuffer, x + x_pos, y + y_pos, colored);
                SetPixel(frameBuffer, x + x_pos, y - y_pos, colored);
                SetPixel(frameBuffer, x - x_pos, y - y_pos, colored);
                DrawHorizontalLine(frameBuffer, x + x_pos, y + y_pos, 2 * (-x_pos) + 1, colored);
                DrawHorizontalLine(frameBuffer, x + x_pos, y - y_pos, 2 * (-x_pos) + 1, colored);

                int e2 = err;
                if (e2 <= y_pos) {
                    y_pos += 1;
                    err += y_pos * 2 + 1;
                    if (-x_pos == y_pos && e2 <= x_pos) {
                        e2 = 0;
                    }
                }
                if (e2 > x_pos) {
                    x_pos  += 1;
                    err += x_pos * 2 + 1;
                }
                if (x_pos > 0) {
                    break;
                }
            }
        }
        #endregion
    }
}