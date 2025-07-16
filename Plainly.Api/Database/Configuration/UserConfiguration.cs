using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plainly.Api.Models;

namespace Plainly.Api.Database.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasColumnType("Date")
                .HasDefaultValueSql("GetDate()");
    }
}