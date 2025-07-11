using Microsoft.EntityFrameworkCore;
using TicTacToe.Application.Configuration;
using TicTacToe.Application.Interfaces;
using TicTacToe.Infrastructure.Persistence;
using TicTacToe.Infrastructure.Services;
using TicTacToe.Application.Features.Games.Commands;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddSingleton<IRandomProvider, SystemRandomProvider>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateGameCommand).Assembly));

builder.Services.Configure<GameSettings>(builder.Configuration.GetSection("GameSettings"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();