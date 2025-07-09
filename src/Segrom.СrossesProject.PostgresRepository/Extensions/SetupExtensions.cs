using Microsoft.Extensions.DependencyInjection;
using Segrom.СrossesProject.Application.Abstractions;
using Segrom.СrossesProject.PostgresRepository.Abstractions;
using Segrom.СrossesProject.PostgresRepository.Factories;

namespace Segrom.СrossesProject.PostgresRepository.Extensions;

public static class SetupExtensions
{
	public static IServiceCollection AddPostgresRepository(this IServiceCollection services)
	{
		services.AddTransient<IGameRepository, PostgresGameRepository>();
		services.AddSingleton<IConnectionFactory, PostgresConnectionFactory>();
		
		return services;
	}
}