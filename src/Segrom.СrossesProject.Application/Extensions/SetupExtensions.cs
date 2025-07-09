using Microsoft.Extensions.DependencyInjection;
using Segrom.СrossesProject.Application.Abstractions;
using Segrom.СrossesProject.Application.Options;

namespace Segrom.СrossesProject.Application.Extensions;

public static class SetupExtensions
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddTransient<IGameService, GameService>();

		services.AddOptions<GameOptions>().BindConfiguration(nameof(GameOptions));
		
		return services;
	}
}