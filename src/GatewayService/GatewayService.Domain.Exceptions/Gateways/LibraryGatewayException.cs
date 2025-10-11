namespace GatewayService.Domain.Exceptions.Gateways;

public class LibraryGatewayException : Exception
{
    public LibraryGatewayException(string message) : base(message) { }
    
    public LibraryGatewayException(string message, Exception innerException) : base(message, innerException) { }
}