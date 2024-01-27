var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); 
builder.Services.AddSingleton<DataNotifier>();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Replace with your React app's origin
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Necessary for SignalR
    });
});

var app = builder.Build();



app.UseSwagger();
app.UseSwaggerUI();
app.MapHub<DataHub>("/DataHub");


var service = new Service();

var cts = new CancellationTokenSource();
Console.CancelKeyPress += (s, e) =>
    {
        Console.WriteLine("Canceling...");
        cts.Cancel();
        e.Cancel = true;
    };

Task.Run(() => service.StartReading(cts.Token));


app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.Run();
