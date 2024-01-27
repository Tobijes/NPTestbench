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


app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.Run();
