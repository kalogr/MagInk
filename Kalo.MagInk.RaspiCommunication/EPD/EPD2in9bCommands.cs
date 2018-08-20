

public static class EPD2in9bCommands 
{
    public static readonly int PANEL_SETTING = 0x00;
    public static readonly int POWER_SETTING = 0x01;
    public static readonly int POWER_OFF = 0x02;
    public static readonly int POWER_OFF_SEQUENCE_SETTING = 0x03;
    public static readonly int POWER_ON = 0x04;
    public static readonly int POWER_ON_MEASURE = 0x05;
    public static readonly int BOOSTER_SOFT_START = 0x06;
    public static readonly int DEEP_SLEEP = 0x07;
    public static readonly int DATA_START_TRANSMISSION_1 = 0x10;
    public static readonly int DATA_STOP = 0x11;
    public static readonly int DISPLAY_REFRESH = 0x12;
    public static readonly int DATA_START_TRANSMISSION_2 = 0x13;
    public static readonly int PLL_CONTROL = 0x30;
    public static readonly int TEMPERATURE_SENSOR_COMMAND = 0x40;
    public static readonly int TEMPERATURE_SENSOR_CALIBRATION = 0x41;
    public static readonly int TEMPERATURE_SENSOR_WRITE = 0x42;
    public static readonly int TEMPERATURE_SENSOR_READ = 0x43;
    public static readonly int VCOM_AND_DATA_INTERVAL_SETTING = 0x50;
    public static readonly int LOW_POWER_DETECTION = 0x51;
    public static readonly int TCON_SETTING = 0x60;
    public static readonly int TCON_RESOLUTION = 0x61;
    public static readonly int GET_STATUS = 0x71;
    public static readonly int AUTO_MEASURE_VCOM = 0x80;
    public static readonly int VCOM_VALUE = 0x81;
    public static readonly int VCM_DC_SETTING_REGISTER = 0x82;
    public static readonly int PARTIAL_WINDOW = 0x90;
    public static readonly int PARTIAL_IN = 0x91;
    public static readonly int PARTIAL_OUT = 0x92;
    public static readonly int PROGRAM_MODE = 0xA0;
    public static readonly int ACTIVE_PROGRAM = 0xA1;
    public static readonly int READ_OTP_DATA = 0xA2;
    public static readonly int POWER_SAVING = 0xE3;
}