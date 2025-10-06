namespace GatewayService.Web.Dto;

public class ValidationErrorResponseDto
{
    public string Message { get; set; } = "Validation failed";
    public Dictionary<string, string> Errors { get; set; } = [];
}