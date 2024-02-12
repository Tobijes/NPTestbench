using Microsoft.AspNetCore.SignalR;
using NPTestbench.Models;

public class DataNotifier
{
    private readonly IHubContext<DataHub> _hubContext;

    public DataNotifier(IHubContext<DataHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public class DataState {
        public int? RunId {get; set;}

        public Dictionary<int, DeviceState> DeviceStates {get; set;} = [];
    }

    public class DeviceState {
        public int Id {get; set;}
        public string Name {get; set;}
        public string? DrawingId {get; set;}
        public float Value {get; set;}
        public float ValueRunMaximum {get; set;}
        public float ValueRunMinimum {get; set;}
    }


    public async Task PublishDataState(DataState dataState)
    {
        await _hubContext.Clients.All.SendAsync("DataState", dataState);
    }
}