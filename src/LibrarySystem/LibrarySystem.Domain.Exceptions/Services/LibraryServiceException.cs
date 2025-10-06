namespace LibrarySystem.Domain.Exceptions.Services;

public class LibraryServiceException : Exception
{
    public LibraryServiceException(string message) : base(message) { }
    
    public LibraryServiceException(string message, Exception innerException) : base(message, innerException) { }
}