using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchTower.Domain.Entity;
using WatchTower.Domain.Shared;

namespace WatchTower.Infrastructure.Configurations;

public class CameraConfiguration : IEntityTypeConfiguration<Camera>
{
    public void Configure(EntityTypeBuilder<Camera> builder)
    {
        builder.ToTable("cameras");

        builder.HasKey(c => c.Id);
        builder.Property(c => c.Title)
            .IsRequired(false)
            .HasMaxLength(Constants.MaxLowTextLength);
        builder.Property(c => c.Ip)
            .IsRequired()
            .HasMaxLength(Constants.MaxIpLength);
        builder.Property(c => c.Name)
            .IsRequired(false);
        builder.Property(c => c.Password)
            .IsRequired(false);
    }
}