using Microsoft.EntityFrameworkCore;
using TicTacToe.Api.SwaggerFilters;
using TicTacToe.Application.Configuration;
using TicTacToe.Application.Features.Games.Commands;
using TicTacToe.Application.Interfaces;
using TicTacToe.Infrastructure.Caching;
using TicTacToe.Infrastructure.Persistence;
using TicTacToe.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddSingleton<IRandomProvider, SystemRandomProvider>();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IIdempotencyCache, InMemoryIdempotencyCache>();

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateGameCommand).Assembly));

builder.Services.Configure<GameSettings>(builder.Configuration.GetSection("GameSettings"));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<IdempotencyKeyHeaderFilter>();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.Run();