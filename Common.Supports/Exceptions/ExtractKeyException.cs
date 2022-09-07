namespace Common.Supports.Exceptions;

public class ExtractKeyException : ArgumentException
{
    public ExtractKeyException()
    {
    }

    public ExtractKeyException(string message) : base(message)
    {
    }

    public ExtractKeyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}