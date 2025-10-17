namespace GatewayService.Domain.Exceptions.Services;

public class RatingServiceNotAvailableServiceException : Exception
{
    public RatingServiceNotAvailableServiceException(string message) : base(message) { }
    
    public RatingServiceNotAvailableServiceException(string message, Exception innerException) : base(message, innerException) { }
}