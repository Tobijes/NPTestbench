using NPTestbench.Models;

public class CalibratedValue {
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public float Value { get; set; }

    public int RunId { get; set; }
    public Run Run { get; set; }

    public int DeviceId { get; set; }
    public Device Device { get; set; }

}