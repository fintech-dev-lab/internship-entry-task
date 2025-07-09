using Segrom.СrossesProject.Application.Extensions;
using Segrom.СrossesProject.Host.Extensions;
using Segrom.СrossesProject.Host.Middleware;
using SetupExtensions = Segrom.СrossesProject.Application.Extensions.SetupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddMemoryCache();
builder.AddSwagger();

var app = builder.Build();

app.UseSwaggerInDevelopment();

app.UseHttpsRedirection();
app.UseHealthChecks("/health");

app.MapControllers();

app.UseMiddleware<ETagMiddleware>();

app.Run();