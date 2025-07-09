namespace Segrom.СrossesProject.PostgresRepository.Exceptions;

public sealed class PostgresRepositoryException: Exception
{
	public PostgresRepositoryException()
	{
	}

	public PostgresRepositoryException(string? message) : base(message)
	{
	}

	public PostgresRepositoryException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}