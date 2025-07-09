using System.Reflection;

namespace Segrom.СrossesProject.Host.Extensions;

internal static class SetupExtensions
{
	public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
	{
		builder.Services.AddOpenApi();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(opts =>
		{
			var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
			opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
		});
		return builder;
	}
	
	public static WebApplication UseSwaggerInDevelopment(this WebApplication app)
	{
		if (app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
			app.UseSwagger();
			app.UseSwaggerUI();
		}
		return app;
	}
}