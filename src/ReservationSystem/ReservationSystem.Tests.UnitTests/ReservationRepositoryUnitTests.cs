using Moq;
using Moq.EntityFrameworkCore;
using ReservationSystem.DataAccess.Context;
using ReservationSystem.DataAccess.Models;
using ReservationSystem.DataAccess.Repositories;
using ReservationSystem.Domain.Interfaces.Repositories;
using ReservationSystem.Domain.Models;
using ReservationSystem.Tests.UnitTests.Factories;

namespace ReservationSystem.Tests.UnitTests;

public class ReservationRepositoryUnitTests
{
    private readonly IReservationRepository _reservationRepository;
    private readonly Mock<ReservationSystemContext> _mockContext = new();

    public ReservationRepositoryUnitTests()
    {
        _reservationRepository = new ReservationRepository(_mockContext.Object);
    }
    
    [Fact]
    public async Task GetReservationsByUsernameAsync_Basic_Ok()
    {
        // Arrange
        var username = "User123";
        var reservationUuid = Guid.NewGuid();
        var bookUuid = Guid.NewGuid();
        var libraryUuid = Guid.NewGuid();
        List<ReservationDb> reservationDbs =
            [ReservationDbFactory.Create(username: username, reservationUuid: reservationUuid, bookUuid: bookUuid, libraryUuid: libraryUuid), ReservationDbFactory.Create()];

        _mockContext.Setup(c => c.Reservations)
            .ReturnsDbSet(reservationDbs);

        List<Reservation> expectedReservations = [ReservationFactory.Create(username: username, reservationUuid: reservationUuid, bookUuid: bookUuid, libraryUuid: libraryUuid)];
        
        // Act
        var actualReservations = await _reservationRepository.GetReservationsByUsernameAsync(username);
        
        // Assert
        Assert.Equal(expectedReservations, actualReservations);
    }

    [Fact]
    public async Task GetReservationsCountByStatusAndUsernameAsync_Basic_Ok()
    {
        // Arrange
        var username = "User123";
        var reservationUuid = Guid.NewGuid();
        var bookUuid = Guid.NewGuid();
        var libraryUuid = Guid.NewGuid();
        var status = ReservationStatusDb.EXPIRED;
        List<ReservationDb> reservationDbs =
            [ReservationDbFactory.Create(username: username, reservationUuid: reservationUuid, bookUuid: bookUuid, libraryUuid: libraryUuid, status: status), ReservationDbFactory.Create(username: username)];
        
        _mockContext.Setup(c => c.Reservations)
            .ReturnsDbSet(reservationDbs);

        var expectedCount = 1;
        
        // Act
        var actualCount = await _reservationRepository.GetReservationsCountByStatusAndUsernameAsync(Domain.Models.ReservationStatus.EXPIRED, username);
        
        // Assert
        Assert.Equal(expectedCount, actualCount);
    }

    [Fact]
    public async Task AddReservationAsync_Basic_Ok()
    {
        // Arrange
        var bookUuid = Guid.NewGuid();
        var libraryUuid = Guid.NewGuid();

        var reservationCreate = ReservationCreateFactory.Create(bookUuid: bookUuid, libraryUuid: libraryUuid);

        _mockContext.Setup(c => c.Reservations)
            .ReturnsDbSet([]);
        
        var expectedStartTime = DateTime.Now;
        var expectedReservation = ReservationFactory.Create(bookUuid: bookUuid, libraryUuid: libraryUuid, id: 1, startDate: new DateOnly(year: expectedStartTime.Year, month: expectedStartTime.Month, day: expectedStartTime.Day));
        
        // Act
        var actualReservation = await _reservationRepository.AddReservationAsync(reservationCreate);
        
        // Assert
        expectedReservation = expectedReservation with { ReservationUuid = actualReservation.ReservationUuid};
        Assert.Equal(expectedReservation, actualReservation);
    }

    [Fact]
    public async Task FindReservationByUuidAsync_Basic_Ok()
    {
        // Arrange
        var bookUuid = Guid.NewGuid();
        var libraryUuid = Guid.NewGuid();
        var reservationUuid = Guid.NewGuid();

        var reservationDb = ReservationDbFactory.Create(bookUuid: bookUuid, libraryUuid: libraryUuid, reservationUuid: reservationUuid);
        
        _mockContext.Setup(c => c.Reservations)
            .ReturnsDbSet([reservationDb]);

        var expectedReservation = ReservationFactory.Create(bookUuid: bookUuid, libraryUuid: libraryUuid,
            reservationUuid: reservationUuid);
        
        // Act
        var actualReservation = await _reservationRepository.FindReservationByUuidAsync(reservationUuid);
        
        // Assert
        Assert.Equal(expectedReservation, actualReservation);
    }

    [Fact]
    public async Task UpdateReservationStatusAsync_Basic_Ok()
    {
        // Arrange
        var bookUuid = Guid.NewGuid();
        var libraryUuid = Guid.NewGuid();
        var reservationUuid = Guid.NewGuid();
        var status = ReservationStatusDb.RENTED;

        var reservationDb = ReservationDbFactory.Create(bookUuid: bookUuid, libraryUuid: libraryUuid, reservationUuid: reservationUuid, status: status);
        
        _mockContext.Setup(c => c.Reservations)
            .ReturnsDbSet([reservationDb]);
        
        var newStatus = Domain.Models.ReservationStatus.EXPIRED;
        var expectedReservation = ReservationFactory.Create(bookUuid: bookUuid, libraryUuid: libraryUuid,
            reservationUuid: reservationUuid, status: newStatus);
        
        // Act
        var actualReservation = await _reservationRepository.UpdateReservationStatusAsync(reservationUuid, newStatus);
        
        // Assert
        Assert.Equal(expectedReservation, actualReservation);
    }
}