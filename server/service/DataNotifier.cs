using Microsoft.AspNetCore.SignalR;

public class DataNotifier
{
    private readonly IHubContext<DataHub> _hubContext;

    public DataNotifier(IHubContext<DataHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task PublishMessage(string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
    }
}