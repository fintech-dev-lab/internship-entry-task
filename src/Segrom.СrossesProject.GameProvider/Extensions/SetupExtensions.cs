using Microsoft.Extensions.DependencyInjection;
using Segrom.СrossesProject.Application.Abstractions;

namespace Segrom.СrossesProject.GameProvider.Extensions;

public static class SetupExtensions
{
	public static IServiceCollection AddGameProvider(this IServiceCollection services)
	{
		services.AddTransient<IGameProvider, DatabaseProviderWithCache>();
		
		return services;
	}
}