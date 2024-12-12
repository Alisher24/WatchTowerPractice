namespace WatchTower.Infrastructure.Configurations;

using Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ShodanCameraConfiguration : IEntityTypeConfiguration<ShodanCamera>
{
    public void Configure(EntityTypeBuilder<ShodanCamera> builder)
    {
        builder.ToTable("shodan_cameras");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Ip)
            .IsRequired();
        builder.Property(s => s.CountryName)
            .IsRequired();
        builder.Property(s => s.CountryCode)
            .IsRequired();
        builder.Property(s => s.City)
            .IsRequired();
        builder.Property(s => s.Latitude)
            .IsRequired();
        builder.Property(s => s.Longitude)
            .IsRequired();

        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}