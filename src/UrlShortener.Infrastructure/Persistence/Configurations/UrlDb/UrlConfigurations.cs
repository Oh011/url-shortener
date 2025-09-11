using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.Entities;

namespace Project.Infrastructure.Persistence.Configurations.UrlDb;
internal class UrlConfigurations : IEntityTypeConfiguration<Url>
{
    public void Configure(EntityTypeBuilder<Url> builder)
    {

        builder.ToTable("Urls");

        // Primary key
        builder.HasKey(u => u.Id);

        // Properties
        builder.Property(u => u.Id);


        builder.Property(u => u.ShortUrl)
               .IsRequired()
               .HasMaxLength(7); // restrict to 7 chars in DB

        builder.HasIndex(u => u.ShortUrl)
               .IsUnique(); // ensure uniqueness

        builder.Property(u => u.OriginalUrl)
               .IsRequired()
               .HasColumnType("text"); // nvarchar(max) equivalent in PostgreSQL

        builder.Property(u => u.ClickCount)
               .HasDefaultValue(0);

        builder.Property(u => u.UserId)
               .HasMaxLength(450); // same as ASP.NET Identity Id

        builder.Property(u => u.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("NOW()"); // PostgreSQL default


        // Optional: index for UserId to speed up queries by user
        builder.HasIndex(u => u.UserId);


        builder.HasMany(u => u.urlAccessLogs)
           .WithOne(l => l.Url)
           .HasForeignKey(l => l.UrlId)
           .OnDelete(DeleteBehavior.Cascade); // delete logs when Url is deleted

    }
}

