namespace GatewayService.Domain.Models.Libraries;

public record LibraryPaged
{
    public int Page { get; set; }
    
    public int PageSize { get; set; }
    
    public int TotalItems { get; set; }
    
    public List<Library> Items { get; set; }
}