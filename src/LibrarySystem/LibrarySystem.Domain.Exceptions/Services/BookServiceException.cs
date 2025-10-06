namespace LibrarySystem.Domain.Exceptions.Services;

public class BookServiceException : Exception
{
    public BookServiceException(string message) : base(message) { }
    
    public BookServiceException(string message, Exception innerException) : base(message, innerException) { }
}