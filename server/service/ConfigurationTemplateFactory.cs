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
public record DeviceTemplate(string Name, ushort StartAddress, DeviceDataType DataType, string DrawingId);
public record ParameterTemplate(string Name, string Value);

public class ConfigurationTemplateFactory
{

    public static Dictionary<ConfigurationTemplateType, string> DefaultNames = new Dictionary<ConfigurationTemplateType, string>{
            {ConfigurationTemplateType.BLANK, "Default: Blank"},
            {ConfigurationTemplateType.STAGE_1_2_VACUUM_CHAMBER, "Default: Stage 1 and 2, vacuum chamber"},
            {ConfigurationTemplateType.STAGE_1_2_ORIFICE_PLATE, "Default: Stage 1 and 2, orifice plate"},
            {ConfigurationTemplateType.STAGE_2_ORIFICE_PLATE, "Default: Stage 2, orifice plate"},
        };
    static DeviceTemplate[] DefaultDevices = [
            new("TF-VC-1", 0, DeviceDataType.Float32, "TF-VC-1"),
            new("PT-VC-1", 2, DeviceDataType.Float32, "PT-VC-1"),

            new("TF-TA-1", 4, DeviceDataType.Float32, "TF-TA-1"),
            new("PT-TA-1", 6, DeviceDataType.Float32, "PT-TA-1"),

            new("TF-S1-1", 8, DeviceDataType.Float32, "TF-S1-1"),
            new("PT-S1-1", 10, DeviceDataType.Float32, "PT-S1-1"),

            new("TF-S1-2", 12, DeviceDataType.Float32, "TF-S1-2"),
            new("PT-S1-2", 14, DeviceDataType.Float32, "PT-S1-2"),

            new("TF-S2-1", 16, DeviceDataType.Float32, "TF-S2-1"),
            new("PT-S2-1", 18, DeviceDataType.Float32, "PT-S2-1"),

            new("TF-S2-2", 20, DeviceDataType.Float32, "TF-S2-2"),
            new("PT-S2-2", 22, DeviceDataType.Float32, "PT-S2-2"),
        ];
    static Dictionary<ConfigurationTemplateType, DeviceTemplate[]> DeviceTemplates = new Dictionary<ConfigurationTemplateType, DeviceTemplate[]>{
        {ConfigurationTemplateType.BLANK, DefaultDevices},
        {ConfigurationTemplateType.STAGE_1_2_VACUUM_CHAMBER, DefaultDevices},
        {ConfigurationTemplateType.STAGE_1_2_ORIFICE_PLATE, DefaultDevices},
        {ConfigurationTemplateType.STAGE_2_ORIFICE_PLATE, DefaultDevices},
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
            StartAddress = template.StartAddress,
            DataType = template.DataType,
            DrawingID = template.DrawingId,
            ConfigurationId = configurationId
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