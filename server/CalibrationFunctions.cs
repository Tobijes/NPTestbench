public class CalibrationFunctions {

    public const string CALIBRATE_PRESSURE_1 = "CALIBRATE_PRESSURE_1";
    public const string CALIBRATE_PRESSURE_11 = "CALIBRATE_PRESSURE_11";
    public const string CALIBRATE_PRESSURE_21 = "CALIBRATE_PRESSURE_21";
    public const string CALIBRATE_TEMPERATURE = "CALIBRATE_TEMPERATURE";
    public const string CALIBRATE_TEMPERATURE_GND = "CALIBRATE_TEMPERATURE_GND";
    public const string CALIBRATE_BITMASK_0001 = "CALIBRATE_BITMASK_0000";
    public const string CALIBRATE_BITMASK_0010 = "CALIBRATE_BITMASK_0010";
    public const string CALIBRATE_BITMASK_0100 = "CALIBRATE_BITMASK_0100";
    public const string CALIBRATE_BITMASK_1000 = "CALIBRATE_BITMASK_1000";

    public static float Calibrate(string calibrationFunctionName, ByteValue[] values) => calibrationFunctionName switch {
        CALIBRATE_PRESSURE_1 => CalibratePressure1(values[0].AsFloat()),
        CALIBRATE_PRESSURE_11 => CalibratePressure11(values[0].AsFloat()),
        CALIBRATE_PRESSURE_21 => CalibratePressure21(values[0].AsFloat()),
        CALIBRATE_TEMPERATURE => CalibrateTemperature(values[0].AsFloat(), values[1].AsFloat()),
        CALIBRATE_TEMPERATURE_GND => CalibrateTemperatureGnd(values[0].AsFloat()),
        CALIBRATE_BITMASK_0001 => CalibrateBitmask(0b00000001, values[0].AsUshort()),
        CALIBRATE_BITMASK_0010 => CalibrateBitmask(0b00000010, values[0].AsUshort()),
        CALIBRATE_BITMASK_0100 => CalibrateBitmask(0b00000100, values[0].AsUshort()),
        CALIBRATE_BITMASK_1000 => CalibrateBitmask(0b00001000, values[0].AsUshort()),
        _ => throw new NotImplementedException()
    };
    
    public static float CalibrateTemperatureGnd(float a) {
        return CalibrateTemperature(a, 0);
    }

    public static float CalibrateTemperature(float a, float b) {
        // Resistance: Voltagedifference a-b over contant amperage 200 mA. Ohm's Law: r=u/i
        float[] c = [-245.19f, 2.5293f, -0.066046f, 4.0422E-3f, -2.0697E-6f, -0.025422f, 1.6883E-3f, -1.3601E-6f];
        float R = Math.Abs(a-b) / 0.02f;
        float T = R * (c[1] + R * (c[2] + R * (c[3] + c[4] * R)))  / (1 + R*(c[5] + R*(c[6] + c[7] * R)));
        return T;
    }

    // Raw is 1-5 V
    public static float CalibratePressure1(float a) {
        return (a - 1) / (5-1);
    }

    // Raw is 1-5 V
    public static float CalibratePressure11(float a) {
        return ((a - 1) / (5-1)) * 11;
    }

    // Raw is 1-5 V
    public static float CalibratePressure21(float a) {
        return ((a - 1) / (5-1)) * 21;
    }

    public static float CalibrateBitmask(byte mask, ushort a) {
        return (ushort) (a & mask) > 0 ? 1.0f : 0.0f;
    }
}