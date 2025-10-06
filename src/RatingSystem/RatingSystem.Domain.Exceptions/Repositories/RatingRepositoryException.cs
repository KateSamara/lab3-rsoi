namespace RatingSystem.Domain.Exceptions.Repositories;

public class RatingRepositoryException : Exception
{
    public RatingRepositoryException(string message) : base(message) { }
    
    public RatingRepositoryException(string message, Exception innerException) : base(message, innerException) { }
}