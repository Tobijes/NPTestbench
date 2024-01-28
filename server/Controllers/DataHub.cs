using Microsoft.AspNetCore.SignalR;

public class DataHub : Hub
{
    public async Task SendMessage(string message)
    {
        Console.WriteLine(message);
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}