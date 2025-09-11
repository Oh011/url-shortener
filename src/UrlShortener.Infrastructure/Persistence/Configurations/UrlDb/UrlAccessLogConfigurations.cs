using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.Infrastructure.Persistence.Configurations.UrlDb
{
    internal class UrlAccessLogConfigurations : IEntityTypeConfiguration<UrlAccessLog>
    {
        public void Configure(EntityTypeBuilder<UrlAccessLog> builder)
        {


            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                   .ValueGeneratedOnAdd(); // auto increment per log row

            builder.Property(l => l.AccessedAt)
                   .IsRequired();

            builder.Property(l => l.IpAddress)
                   .IsRequired()
                   .HasMaxLength(45); // IPv6 safe

            builder.Property(l => l.UserAgent)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(l => l.Referrer)
                   .HasMaxLength(512);

        }
    }
}
