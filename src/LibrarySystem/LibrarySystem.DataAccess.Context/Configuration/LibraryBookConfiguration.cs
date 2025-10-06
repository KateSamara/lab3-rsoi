using LibrarySystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.DataAccess.Context.Configuration;

public class LibraryBookConfiguration : IEntityTypeConfiguration<LibraryBookDb>
{
    public void Configure(EntityTypeBuilder<LibraryBookDb> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.BookId).IsRequired();
        builder.Property(x => x.LibraryId).IsRequired();
        builder.Property(x => x.AvailableCount).IsRequired();
        
        builder.HasOne(x => x.Book)
            .WithMany(x => x.Libraries)
            .HasForeignKey(x => x.BookId);
        builder.HasOne(x => x.Library)
            .WithMany(x => x.Books)
            .HasForeignKey(x => x.LibraryId);
    }
}