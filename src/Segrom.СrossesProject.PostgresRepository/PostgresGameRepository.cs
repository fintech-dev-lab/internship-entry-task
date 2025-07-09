using Dapper;
using Microsoft.Extensions.Logging;
using Segrom.СrossesProject.Application.Abstractions;
using Segrom.СrossesProject.Domain.Entities;
using Segrom.СrossesProject.PostgresRepository.Abstractions;
using Segrom.СrossesProject.PostgresRepository.DAO;
using Segrom.СrossesProject.PostgresRepository.Exceptions;
using Segrom.СrossesProject.PostgresRepository.Extensions;

namespace Segrom.СrossesProject.PostgresRepository;

internal sealed class PostgresGameRepository(
	ILogger<PostgresGameRepository> logger, 
	IConnectionFactory connectionFactory
	): IGameRepository
{
	public async Task Create(Game game, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		try
		{
			const string sql = 
				"""
				INSERT INTO games (
				       id,
				       created_at,
				       updated_at,
				       state,
				       winner,
				       current_player,
				       cells,
				       length_for_win
				)
				VALUES (
				       @Id,
				       @Created,
				       @Updated,
				       @State,
				       @Winner,
				       @CurrentPlayer,
				       @Cells,
				       @LengthForWin
				);
				""";
			
			var parameters = new
			{
				game.Id, 
				State = (short)game.State, 
				Winner = (short?)game.Winner, 
				CurrentPlayer = (short)game.CurrentPlayer, 
				Cells = game.Field.Cells.ToShortArray(), 
				LengthForWin = (short)game.LengthForWin,
				Created = DateTime.UtcNow, 
				Updated = DateTime.UtcNow,
			};
			
			await using var connection = connectionFactory.Get();
			var cmd = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);
			var affectedRows = await connection.ExecuteAsync(cmd);

			if (affectedRows == 0)
			{
				logger.LogWarning("Insert new game not affected rows");
			}
		}
		catch (Exception e)
		{
			throw new PostgresRepositoryException("Failed to insert new game", e);
		}
	}

	public async Task<Game?> Get(Guid gameId, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		try
		{
			const string sql = 
				"""
				SELECT id AS Id, 
				       created_at AS Created,
				       updated_at AS Updated,
				       state AS State,
				       winner AS Winner,
				       current_player AS CurrentPlayer,
				       cells AS Cells,
				       length_for_win AS LengthForWin
				FROM games
				WHERE id = @id
				LIMIT 1;
				""";
			
			var parameters = new { id = gameId };
			
			await using var connection = connectionFactory.Get();
			var cmd = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);
			var dao = await connection.QueryFirstAsync<GameDao>(cmd);
			
			return dao.ToDomain();
		}
		catch (Exception e)
		{
			throw new PostgresRepositoryException("Failed to find game by id", e);
		}
	}

	public async Task Update(Game game, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();
		try
		{
			const string sql = 
				"""
				UPDATE games
				SET 
				    updated_at = @Updated,
				    state = @State,
				    winner = @Winner,
				    current_player = @CurrentPlayer,
				    cells = @Cells,
				    length_for_win = @LengthForWin
				WHERE id = @Id
				;
				""";
			
			var parameters = new
			{
				game.Id, game.State, game.Winner, game.CurrentPlayer, game.Field.Cells, game.LengthForWin,
				Updated = DateTime.UtcNow,
			};
			
			await using var connection = connectionFactory.Get();
			var cmd = new CommandDefinition(sql, parameters, cancellationToken: cancellationToken);
			var affectedRows = await connection.ExecuteAsync(cmd);

			if (affectedRows == 0)
			{
				logger.LogWarning("Update game not affected rows");
			}
		}
		catch (Exception e)
		{
			throw new PostgresRepositoryException("Failed to update game", e);
		}
	}
}