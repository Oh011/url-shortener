using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;
using Project.Infrastructure.Identity.Entities;

namespace Project.Infrastructure.Persistence.Configurations.AppDb
{
    internal class UserStatisticsConfigurations : IEntityTypeConfiguration<UserStatistics>
    {
        public void Configure(EntityTypeBuilder<UserStatistics> builder)
        {
            builder.HasKey(s => s.UserId);




            builder.Property(s => s.UserId)
                   .IsRequired()
                   .HasMaxLength(450); // matches AspNetUsers.Id length




            builder.Property(s => s.TotalUrls)
                   .HasDefaultValue(0);

            builder.Property(s => s.TotalClicks)
                   .HasDefaultValue(0);

            builder.Property(s => s.LastActiveAt)
                   .HasDefaultValueSql("GETUTCDATE()"); // server default in SQL Server

            // 1-to-1 relation with ApplicationUser
            builder.HasOne<ApplicationUser>()
                   .WithOne(u => u.Statistics)
                   .HasForeignKey<UserStatistics>(s => s.UserId)
                   .OnDelete(DeleteBehavior.Cascade); // delete stats if user deleted
        }
    }
}
