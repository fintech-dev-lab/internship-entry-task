namespace Segrom.СrossesProject.Domain.Exceptions;

public sealed class DomainException: Exception
{
	public DomainException()
	{
	}

	public DomainException(string? message) : base(message)
	{
	}

	public DomainException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}