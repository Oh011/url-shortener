using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Identity.Entities;

namespace UrlShortener.Infrastructure.Persistence.Configurations.AppDb
{
    public class RefreshTokenConfigurations : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshTokens");

            // Primary Key
            builder.HasKey(rt => rt.Id);

            // Properties
            builder.Property(rt => rt.Token)
                   .IsRequired()
                   .HasMaxLength(450); // length suitable for GUID or secure random strings

            builder.Property(rt => rt.DeviceId)
                   .IsRequired()
                   .HasMaxLength(200); // reasonable length for device identifiers

            builder.Property(rt => rt.UserId)
                   .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                   .IsRequired();

            builder.Property(rt => rt.ExpiresAt)
                   .IsRequired();

            // Indexes for frequent searches
            builder.HasIndex(rt => new { rt.Token, rt.DeviceId })
                   .IsUnique(); // A device cannot reuse the same refresh token

            builder.HasIndex(rt => rt.UserId);

            // Relationships

        }
    }
}
