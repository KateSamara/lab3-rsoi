using ReservationSystem.Domain.Models;

namespace ReservationSystem.Tests.UnitTests.Factories;

public static class ReservationCreateFactory
{
    public static ReservationCreate Create(string username = "Username",
        Guid? bookUuid = null,
        Guid? libraryUuid = null,
        DateOnly? tillDate = null)
    {
        return new ReservationCreate
        {
            Username = username,
            BookUuid = bookUuid ?? Guid.NewGuid(),
            LibraryUuid = libraryUuid ?? Guid.NewGuid(),
            TillDate = tillDate ?? new DateOnly()
        };
    }
}