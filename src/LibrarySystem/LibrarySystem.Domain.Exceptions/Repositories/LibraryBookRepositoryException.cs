namespace LibrarySystem.Domain.Exceptions.Repositories;

public class LibraryBookRepositoryException : Exception
{
    public LibraryBookRepositoryException(string message) : base(message) { }
    
    public LibraryBookRepositoryException(string message, Exception innerException) : base(message, innerException) { }
}