using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;
using Project.Infrastructure.Identity.Entities;

namespace Project.Infrastructure.Persistence.Configurations.AppDb
{
    public class UserAnalyticsConfiguration : IEntityTypeConfiguration<UserAnalytics>
    {
        public void Configure(EntityTypeBuilder<UserAnalytics> builder)
        {
            builder.ToTable("UserAnalytics");

            builder.HasKey(u => new { u.UserId, u.ShortUrl });
            // ✅ Composite PK since a user can’t have two identical short URLs

            builder.Property(u => u.UserId)
                   .IsRequired();

            builder.Property(u => u.ShortUrl)
                   .HasMaxLength(20) // assuming short codes are short
                   .IsRequired();

            builder.Property(u => u.OriginalUrl)
                   .IsRequired();


            builder.Property(u => u.ClickCount)
                   .HasDefaultValue(0);

            builder.Property(u => u.CreatedAt)
                   .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(u => u.LastAccessedAt)
                   .IsRequired(false);

            // 🔗 Relation with ApplicationUser
            builder.HasOne<ApplicationUser>()
                   .WithMany(u => u.UserAnalytics)
                   .HasForeignKey(u => u.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
            // ✅ delete analytics when user is deleted
        }
    }
}
