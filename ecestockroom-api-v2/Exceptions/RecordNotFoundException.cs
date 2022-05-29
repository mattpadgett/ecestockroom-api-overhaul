namespace ecestockroom_api_v2.Exceptions;

public class RecordNotFoundException : Exception
{
    public RecordNotFoundException()
    {
    }

    public RecordNotFoundException(string? message) : base(message)
    {
    }

    public RecordNotFoundException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}