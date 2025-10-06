using LibrarySystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.DataAccess.Context.Configuration;

public class LibraryConfiguration : IEntityTypeConfiguration<LibraryDb>
{
    public void Configure(EntityTypeBuilder<LibraryDb> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.LibraryUuid).IsRequired();
        builder.HasIndex(x => x.LibraryUuid).IsUnique();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(80);
        builder.Property(x => x.City).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Address).IsRequired().HasMaxLength(255);
        
        builder.HasMany(x => x.Books)
            .WithOne(x => x.Library)
            .HasForeignKey(x => x.LibraryId);
    }
}