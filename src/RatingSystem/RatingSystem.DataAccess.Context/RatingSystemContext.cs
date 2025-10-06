using Microsoft.EntityFrameworkCore;
using RatingSystem.DataAccess.Context.Configuration;
using RatingSystem.DataAccess.Models;

namespace RatingSystem.DataAccess.Context;

public class RatingSystemContext : DbContext
{
    public virtual DbSet<RatingDb> Ratings { get; set; }
    
    public RatingSystemContext(DbContextOptions<RatingSystemContext> options) : base(options) { }
    
    public RatingSystemContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new RatingConfiguration());
    }
}