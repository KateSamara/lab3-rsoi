namespace GatewayService.Domain.Exceptions.Services;

public class RatingServiceException : Exception
{
    public RatingServiceException(string message) : base(message) { }
    
    public RatingServiceException(string message, Exception innerException) : base(message, innerException) { }
}