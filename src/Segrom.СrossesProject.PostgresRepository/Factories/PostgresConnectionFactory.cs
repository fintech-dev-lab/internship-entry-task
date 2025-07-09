using Microsoft.Extensions.Configuration;
using Npgsql;
using Segrom.СrossesProject.PostgresRepository.Abstractions;

namespace Segrom.СrossesProject.PostgresRepository.Factories;

internal sealed class PostgresConnectionFactory(IConfiguration configuration): IConnectionFactory
{
	private readonly string _connectionString = configuration.GetConnectionString("Postgres") 
	                                            ?? throw new NullReferenceException("Postgres connection string not found");
	
	public NpgsqlConnection Get()
	{
		return new NpgsqlConnection(_connectionString);
	}
}