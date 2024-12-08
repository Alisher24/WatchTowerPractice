using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchTower.Domain.Entity;
using WatchTower.Domain.Shared;

namespace WatchTower.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(Constants.MaxLowTextLength);
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(Constants.MaxEmailLength);
        builder.Property(u => u.Key)
            .IsRequired(false); 
        builder.Property(u => u.IsActive)
            .IsRequired();
        builder.Property(u => u.Token)
            .IsRequired(false);
        builder.Property(u => u.Password)
            .IsRequired();

        builder.HasMany(u => u.Cameras)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .HasPrincipalKey(u => u.Id);
    }
}