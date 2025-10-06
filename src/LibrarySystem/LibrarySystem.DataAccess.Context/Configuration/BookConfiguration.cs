using LibrarySystem.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibrarySystem.DataAccess.Context.Configuration;

public class BookConfiguration : IEntityTypeConfiguration<BookDb>
{
    public void Configure(EntityTypeBuilder<BookDb> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.HasIndex(x => x.BookUuid).IsUnique();
        builder.Property(x => x.BookUuid).IsRequired();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Author).HasMaxLength(255);
        builder.Property(x => x.Genre).HasMaxLength(255);
        builder.Property(x => x.Condition).IsRequired().HasDefaultValue(BookConditionDb.EXCELLENT);
        
        builder.HasMany(x => x.Libraries)
            .WithOne(x => x.Book)
            .HasForeignKey(x => x.BookId);
    }
}