using NPTestbench.Models;

namespace NPTestbench.Service.Templates;


public enum ConfigurationTemplateType
{
    BLANK,
    STAGE_1_2_VACUUM_CHAMBER,
    STAGE_1_2_ORIFICE_PLATE,
    STAGE_2_ORIFICE_PLATE
}

// Intermediate objects to ease manual writing
public record ChannelTemplate(string Name, ushort Address, bool Writable, DeviceDataType DataType);
public record DeviceTemplate(string Name, string? DrawingId, string CalibrationFunctionName);
public record DeviceChannelTemplate(string DeviceName, string ChannelName, bool IsRead, int Order);
public record ParameterTemplate(string Name, string Value);

public class ConfigurationTemplateFactory
{

    public static Dictionary<ConfigurationTemplateType, string> DefaultNames = new Dictionary<ConfigurationTemplateType, string>{
            // {ConfigurationTemplateType.BLANK, "Default: Blank"},
            {ConfigurationTemplateType.STAGE_1_2_VACUUM_CHAMBER, "Default: Stage 1 and 2, vacuum chamber"},
            {ConfigurationTemplateType.STAGE_1_2_ORIFICE_PLATE, "Default: Stage 1 and 2, orifice plate"},
            {ConfigurationTemplateType.STAGE_2_ORIFICE_PLATE, "Default: Stage 2, orifice plate"},
        };

    static ChannelTemplate[] DefaultChannels = [
        new("AIN4", 8, false, DeviceDataType.Float32),
        new("AIN5", 10, false, DeviceDataType.Float32),
        new("AIN6", 12, false, DeviceDataType.Float32),
        new("AIN7", 14, false, DeviceDataType.Float32),
        new("AIN8", 16, false, DeviceDataType.Float32),
        new("AIN9", 18, false, DeviceDataType.Float32),
        new("AIN10", 20, false, DeviceDataType.Float32),
        new("AIN11", 22, false, DeviceDataType.Float32),
        new("AIN12", 24, false, DeviceDataType.Float32),
        new("AIN13", 26, false, DeviceDataType.Float32),
        new("CIO2", 2018, true, DeviceDataType.UInt16),
        new("CIO3", 2019, true, DeviceDataType.UInt16),
    ];

    static DeviceTemplate[] DefaultDevices = [
            // Read-only devices
            new("TF-S1-1", "TF-S1-1", CalibrationFunctions.CALIBRATE_TEMPERATURE),
            new("PT-S1-1", "PT-S1-1", CalibrationFunctions.CALIBRATE_PRESSURE_11),

            new("TF-S1-2", "TF-S1-2", CalibrationFunctions.CALIBRATE_TEMPERATURE),
            new("PT-S1-2", "PT-S1-2", CalibrationFunctions.CALIBRATE_PRESSURE_1),

            new("TF-S2-1", "TF-S2-1", CalibrationFunctions.CALIBRATE_TEMPERATURE),
            new("PT-S2-1", "PT-S2-1", CalibrationFunctions.CALIBRATE_PRESSURE_11),

            new("TF-S2-2", "TF-S2-2", CalibrationFunctions.CALIBRATE_TEMPERATURE),
            new("PT-S2-2", "PT-S2-2", CalibrationFunctions.CALIBRATE_PRESSURE_1),

            new("TF-TA-1", "TF-TA-1", CalibrationFunctions.CALIBRATE_TEMPERATURE_GND),
            new("PT-TA-1", "PT-TA-1", CalibrationFunctions.CALIBRATE_PRESSURE_21),

            // new("TF-VC-1", "TF-VC-1", CalibrationFunctions.CALIBRATE_TEMPERATURE),
            // new("PT-VC-1", "PT-VC-1", CalibrationFunctions.CALIBRATE_PRESSURE),

            // Read-write devices
            new("VS-S1-1", null, CalibrationFunctions.CALIBRATE_SIMPLE),
            new("VS-S2-1", null, CalibrationFunctions.CALIBRATE_SIMPLE),
        ];

    static DeviceChannelTemplate[] DefaultDeviceChannels = [
        new("TF-S1-1", "AIN7", true, 0),
        new("TF-S1-1", "AIN8", true, 1),

        new("PT-S1-1", "AIN13", true, 0),

        new("TF-S1-2", "AIN6", true, 0),
        new("TF-S1-2", "AIN7", true, 1),

        new("PT-S1-2", "AIN12", true, 0),

        new("VS-S1-1", "CIO2", true, 0),
        new("VS-S1-1", "CIO2", false, 0),

        new("TF-S2-1", "AIN5", true, 0),
        new("TF-S2-1", "AIN6", true, 1),

        new("PT-S2-1", "AIN11", true, 0),

        new("TF-S2-2", "AIN4", true, 0),
        new("TF-S2-2", "AIN5", true, 1),

        new("PT-S2-2", "AIN10", true, 0),

        new("VS-S2-1", "CIO3", true, 0),
        new("VS-S2-1", "CIO3", false, 0),
        
        new("TF-TA-1", "AIN4", true, 0),

        new("PT-TA-1", "AIN9", true, 0)
    ];
    static Dictionary<ConfigurationTemplateType, DeviceTemplate[]> DeviceTemplates = new Dictionary<ConfigurationTemplateType, DeviceTemplate[]>{
        {ConfigurationTemplateType.BLANK, DefaultDevices},
        {ConfigurationTemplateType.STAGE_1_2_VACUUM_CHAMBER, DefaultDevices},
        {ConfigurationTemplateType.STAGE_1_2_ORIFICE_PLATE, DefaultDevices},
        {ConfigurationTemplateType.STAGE_2_ORIFICE_PLATE, DefaultDevices},
    };

    static Dictionary<ConfigurationTemplateType, ChannelTemplate[]> ChannelTemplates = new Dictionary<ConfigurationTemplateType, ChannelTemplate[]>{
        {ConfigurationTemplateType.BLANK, DefaultChannels},
        {ConfigurationTemplateType.STAGE_1_2_VACUUM_CHAMBER, DefaultChannels},
        {ConfigurationTemplateType.STAGE_1_2_ORIFICE_PLATE, DefaultChannels},
        {ConfigurationTemplateType.STAGE_2_ORIFICE_PLATE, DefaultChannels},
    };

    static Dictionary<ConfigurationTemplateType, DeviceChannelTemplate[]> DeviceChannelTemplates = new Dictionary<ConfigurationTemplateType, DeviceChannelTemplate[]>{
        {ConfigurationTemplateType.BLANK, DefaultDeviceChannels},
        {ConfigurationTemplateType.STAGE_1_2_VACUUM_CHAMBER, DefaultDeviceChannels},
        {ConfigurationTemplateType.STAGE_1_2_ORIFICE_PLATE, DefaultDeviceChannels},
        {ConfigurationTemplateType.STAGE_2_ORIFICE_PLATE, DefaultDeviceChannels},
    };

    static string[] DefaultParameterNames = [
        "Stage 1 feed pressure set point [bar(g)]",
        "Stage 1 nozzle [- / string]",
        "Stage 1 NXP [mm]",
        "Stage 1 dmc [mm]",
        "Stage 1 lmc [mm]",
        "Stage 1 orifice plate [mm2]",
        "Stage 2 feed pressure set point [bar(g)]",
        "Stage 2 nozzle [- / string]",
        "Stage 2 NXP [mm]",
        "Stage 2 dmc [mm]",
        "Stage 2 lmc [mm]",
        "Stage 2 orifice plate [mm2]"
    ];

    static Dictionary<ConfigurationTemplateType, ParameterTemplate[]> ParameterTemplates = new Dictionary<ConfigurationTemplateType, ParameterTemplate[]>{
        {ConfigurationTemplateType.BLANK, []},
        {ConfigurationTemplateType.STAGE_1_2_VACUUM_CHAMBER, [
            new(DefaultParameterNames[0], ""),
            new(DefaultParameterNames[1], ""),
            new(DefaultParameterNames[2], ""),
            new(DefaultParameterNames[3], ""),
            new(DefaultParameterNames[4], ""),
            // Not [5]
            new(DefaultParameterNames[6], ""),
            new(DefaultParameterNames[7], ""),
            new(DefaultParameterNames[8], ""),
            new(DefaultParameterNames[9], ""),
            new(DefaultParameterNames[10], ""),
            // Not [11]
        ]},
        {ConfigurationTemplateType.STAGE_1_2_ORIFICE_PLATE, [
            new(DefaultParameterNames[0], ""),
            new(DefaultParameterNames[1], ""),
            new(DefaultParameterNames[2], ""),
            new(DefaultParameterNames[3], ""),
            new(DefaultParameterNames[4], ""),
            new(DefaultParameterNames[5], ""),
            new(DefaultParameterNames[6], ""),
            new(DefaultParameterNames[7], ""),
            new(DefaultParameterNames[8], ""),
            new(DefaultParameterNames[9], ""),
            new(DefaultParameterNames[10], ""),
            // Not [11]
        ]},
        {ConfigurationTemplateType.STAGE_2_ORIFICE_PLATE, [
            // Not [0,1,2,3,4,5]
            new(DefaultParameterNames[6], ""),
            new(DefaultParameterNames[7], ""),
            new(DefaultParameterNames[8], ""),
            new(DefaultParameterNames[9], ""),
            new(DefaultParameterNames[10], ""),
            new(DefaultParameterNames[11], ""),
        ]},
    };

    public static Device[] GenerateTemplateDevices(ConfigurationTemplateType type, int configurationId)
    {
        return DeviceTemplates[type].Select(template => new Device()
        {
            Name = template.Name,
            DrawingID = template.DrawingId,
            CalibrationFunctionName = template.CalibrationFunctionName,
            ConfigurationId = configurationId
        }).ToArray();
    }

    public static Channel[] GenerateTemplateChannels()
    {   
        return DefaultChannels.Select(template => new Channel()
        {
            Name = template.Name,
            Address = template.Address,
            DataType = template.DataType,
            Writable = template.Writable
        }).ToArray();
    }

    public static DeviceChannel[] GenerateTemplateDeviceChannels(ConfigurationTemplateType type, Device[] devices, Channel[] channels)
    {
        return DeviceChannelTemplates[type].Select(template => new DeviceChannel()
        {
            DeviceId = devices.Where(d => d.Name == template.DeviceName).First().Id,
            ChannelId = channels.Where(d => d.Name == template.ChannelName).First().Id,
            IsRead = template.IsRead,
            Order = template.Order
        }).ToArray();
    }

    public static Parameter[] GenerateTemplateParameters(ConfigurationTemplateType type, int configurationId)
    {
         return ParameterTemplates[type].Select(template => new Parameter()
        {
            Name = template.Name,
            Value = template.Value,
            ConfigurationId = configurationId
        }).ToArray();
    }

}