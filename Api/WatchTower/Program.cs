using Microsoft.EntityFrameworkCore;
using WatchTower.Database;
using WatchTower.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WatchTowerDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StreamService>();
builder.Services.AddScoped<CameraService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

var app = builder.Build();

app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.UseWebSockets();

app.Run();