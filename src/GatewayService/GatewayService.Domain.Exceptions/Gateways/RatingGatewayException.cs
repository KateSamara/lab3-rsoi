namespace GatewayService.Domain.Exceptions.Gateways;

public class RatingGatewayException : Exception
{
    public RatingGatewayException(string message) : base(message) { }
    
    public RatingGatewayException(string message, Exception innerException) : base(message, innerException) { }
}