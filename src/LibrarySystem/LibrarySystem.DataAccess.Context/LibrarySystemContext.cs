using LibrarySystem.DataAccess.Context.Configuration;
using LibrarySystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.DataAccess.Context;

public class LibrarySystemContext : DbContext
{
    public virtual DbSet<BookDb> Books { get; set; }
    public virtual DbSet<LibraryDb> Libraries { get; set; }
    public virtual DbSet<LibraryBookDb> LibraryBooks { get; set; }
    
    public LibrarySystemContext(DbContextOptions<LibrarySystemContext> options) : base(options) { }
    
    public LibrarySystemContext() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new LibraryConfiguration());
        modelBuilder.ApplyConfiguration(new LibraryBookConfiguration());
    }
}