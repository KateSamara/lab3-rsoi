using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationSystem.DataAccess.Models;

namespace ReservationSystem.DataAccess.Context;

public class ReservationConfiguration : IEntityTypeConfiguration<ReservationDb>
{
    public void Configure(EntityTypeBuilder<ReservationDb> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.ReservationUuid).IsRequired();
        builder.HasIndex(x => x.ReservationUuid).IsUnique();
        builder.Property(x => x.Username).IsRequired().HasMaxLength(80);
        builder.Property(x => x.BookUuid).IsRequired();
        builder.Property(x => x.LibraryUuid).IsRequired();
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.TillDate).IsRequired();
    }
}