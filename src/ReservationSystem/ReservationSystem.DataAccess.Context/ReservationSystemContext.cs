using Microsoft.EntityFrameworkCore;
using ReservationSystem.DataAccess.Models;

namespace ReservationSystem.DataAccess.Context;

public class ReservationSystemContext : DbContext
{
    public virtual DbSet<ReservationDb> Reservations { get; set; }
    
    public ReservationSystemContext(DbContextOptions<ReservationSystemContext> options) : base(options) { }
    
    public ReservationSystemContext() { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ReservationConfiguration());
    }
}