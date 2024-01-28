using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); 
builder.Services.AddSingleton<DataNotifier>();
builder.Services.AddSingleton<ConfigurationService>();;
builder.Services.AddHostedService<DataService>();
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

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
});

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapHub<DataHub>("/DataHub");
app.MapControllers();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.Run();
