namespace LibrarySystem.Domain.Exceptions.Services;

public class LibraryBookServiceException : Exception
{
    public LibraryBookServiceException(string message) : base(message) { }
    
    public LibraryBookServiceException(string message, Exception innerException) : base(message, innerException) { }
}