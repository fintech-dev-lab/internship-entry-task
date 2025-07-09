using Segrom.СrossesProject.Application.Extensions;
using Segrom.СrossesProject.GameProvider.Extensions;
using Segrom.СrossesProject.Host.Extensions;
using Segrom.СrossesProject.Host.Middleware;
using Segrom.СrossesProject.PostgresRepository.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHealthChecks();

builder.Services.AddApplication();
builder.Services.AddGameProvider();
builder.Services.AddMemoryCache();
builder.Services.AddPostgresRepository();
builder.AddSwagger();

var app = builder.Build();

app.UseSwaggerInDevelopment();

app.UseHttpsRedirection();
app.UseHealthChecks("/health");

app.MapControllers();

app.UseMiddleware<ETagMiddleware>();

app.ApplyPostgresMigrations();

app.Run();