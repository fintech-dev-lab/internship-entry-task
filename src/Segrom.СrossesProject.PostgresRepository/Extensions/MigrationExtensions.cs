using FluentMigrator.Runner;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Segrom.СrossesProject.PostgresRepository.Exceptions;

namespace Segrom.СrossesProject.PostgresRepository.Extensions;

public static class MigrationExtensions
{
	public static IApplicationBuilder ApplyPostgresMigrations(this IApplicationBuilder app)
	{

		var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
		if (configuration.GetValue<bool>("SkipMigrations")) 
			return app;
		var connectionString = configuration.GetConnectionString("Postgres")
		                       ?? throw new PostgresRepositoryException("No connection string configured.");
		
		var serviceProvider = CreateMigrationServices(connectionString);
		using var scope = serviceProvider.CreateScope();
		var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

		if (configuration.GetValue<string>("RollbackMigrations")?.ToLower() == "true")
		{
			runner.MigrateDown(0);
		}
		else
		{
			runner.MigrateUp();
		}
		
		return app;
	}
	
	private static IServiceProvider CreateMigrationServices(string connectionString)
		=> new ServiceCollection()
			.AddFluentMigratorCore()
			.ConfigureRunner(builder => builder
				.AddPostgres()
				.WithGlobalConnectionString(connectionString)
				.ScanIn(typeof(MigrationExtensions).Assembly).For.Migrations()
				.ConfigureGlobalProcessorOptions(op => op.ProviderSwitches = "Force Quote=false"))
			.AddLogging(log => log.AddFluentMigratorConsole())
			.BuildServiceProvider(false);
}