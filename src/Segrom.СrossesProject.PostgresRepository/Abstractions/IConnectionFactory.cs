using Npgsql;

namespace Segrom.СrossesProject.PostgresRepository.Abstractions;

internal interface IConnectionFactory
{
	NpgsqlConnection Get();
}