namespace LibrarySystem.Domain.Exceptions.Repositories;

public class LibraryRepositoryException : Exception
{
    public LibraryRepositoryException(string message) : base(message) { }
    
    public LibraryRepositoryException(string message, Exception innerException) : base(message, innerException) { }
}