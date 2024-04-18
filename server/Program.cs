using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using NPTestbench.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSingleton<DataNotifier>();
builder.Services.AddSingleton<ConfigurationService>();
builder.Services.AddSingleton<CommunicationService>();
builder.Services.AddSingleton<DataContext>();
builder.Services.AddSingleton<DataService>();
builder.Services.AddHostedService<DataService>(p => p.GetRequiredService<DataService>());
// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173") // Replace with your React app's origin
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Necessary for SignalR
    });
});

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    context.Database.EnsureCreated(); 
}
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapHub<DataHub>("/DataHub");
app.MapControllers();

app.UseCors("CorsPolicy");

app.Run();
