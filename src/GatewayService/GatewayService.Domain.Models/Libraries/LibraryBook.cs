using GatewayService.Domain.Models.Books;

namespace GatewayService.Domain.Models.Libraries;

public record LibraryBook
{
    public required int AvailableCount { get; init; }

    public required Library Library { get; init; }
   
    public required Book Book { get; init; }
}