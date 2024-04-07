using NPTestbench.Models;

public class DeviceChannel {
    public int Id { get; set; }
    public bool IsRead { get; set; }
    public int Order { get; set; } 

    // Relation
    public int DeviceId { get; set; }
    public Device Device { get; set; }

    public int ChannelId { get; set; }
    public Channel Channel { get; set; }
}