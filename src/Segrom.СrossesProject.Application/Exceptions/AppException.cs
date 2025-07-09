namespace Segrom.СrossesProject.Application.Exceptions;

public sealed class AppException:  Exception
{
	public AppException()
	{
	}

	public AppException(string? message) : base(message)
	{
	}

	public AppException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}