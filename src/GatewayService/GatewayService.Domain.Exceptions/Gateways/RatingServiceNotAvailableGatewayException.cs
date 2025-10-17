namespace GatewayService.Domain.Exceptions.Gateways;

public class RatingServiceNotAvailableGatewayException : Exception
{
    public RatingServiceNotAvailableGatewayException(string message) : base(message) { }
    
    public RatingServiceNotAvailableGatewayException(string message, Exception innerException) : base(message, innerException) { }
}