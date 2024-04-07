public class CalibrationFunctions {

    public const string CALIBRATE_PRESSURE_1 = "CALIBRATE_PRESSURE_1";
    public const string CALIBRATE_PRESSURE_11 = "CALIBRATE_PRESSURE_11";
    public const string CALIBRATE_PRESSURE_21 = "CALIBRATE_PRESSURE_21";
    public const string CALIBRATE_TEMPERATURE = "CALIBRATE_TEMPERATURE";
    public const string CALIBRATE_TEMPERATURE_GND = "CALIBRATE_TEMPERATURE_GND";
    public const string CALIBRATE_SIMPLE = "CALIBRATE_SIMPLE";

    public static float Calibrate(string calibrationFunctionName, float[] values) => calibrationFunctionName switch {
        CALIBRATE_PRESSURE_1 => CalibratePressure1(values[0]),
        CALIBRATE_PRESSURE_11 => CalibratePressure11(values[0]),
        CALIBRATE_PRESSURE_21 => CalibratePressure21(values[0]),
        CALIBRATE_TEMPERATURE => CalibrateTemperature(values[0], values[1]),
        CALIBRATE_TEMPERATURE_GND => CalibrateTemperatureGnd(values[0]),
        CALIBRATE_SIMPLE => CalibrateSimple(values[0]),
        _ => throw new NotImplementedException()
    };
    
    public static float CalibrateTemperatureGnd(float a) {
        return CalibrateTemperature(a, 0);
    }

    public static float CalibrateTemperature(float a, float b) {
        // Resistance: Voltagedifference a-b over contant amperage 200 mA. Ohm's Law: r=u/i
        float[] c = [-245.19f, 2.5293f, -0.066046f, 4.0422E-3f, -2.0697E-6f, -0.025422f, 1.6883E-3f, -1.3601E-6f];
        float R = (a-b) / 0.2f;
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

    public static float CalibrateSimple(float a) {
        return a;
    }
}