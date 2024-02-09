using Microsoft.AspNetCore.SignalR;

public class DataNotifier
{
    private readonly IHubContext<DataHub> _hubContext;

    public DataNotifier(IHubContext<DataHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public class DataMessage {
        public int DeviceId {get; set;}
        public string? DrawingId { get; set; }
        public float Value {get; set;}
    }

    public async Task PublishMessage(DataMessage message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}