using Microsoft.EntityFrameworkCore;
using TicTacToe.Contracts.AutoMapper;
using TicTacToe.Core.Entities;
using TicTacToe.Services.Repository;
using TicTacToe.Services.Repository.Interfaces;
using TicTacToe.Services.Service;
using TicTacToe.Services.Service.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<GameSettings>(builder.Configuration.GetSection("GameSettings"));

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IGameService, GameService>();

builder.Services.AddScoped<IMoveRepository, MoveRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDbContext<TicTacToeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.MapControllers();
app.Run();