using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RatingSystem.DataAccess.Models;

namespace RatingSystem.DataAccess.Context.Configuration;

public class RatingConfiguration : IEntityTypeConfiguration<RatingDb>
{
    public void Configure(EntityTypeBuilder<RatingDb> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.Username).IsRequired().HasMaxLength(80);
        builder.Property(x => x.Stars).IsRequired();
    }
}