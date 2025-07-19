using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plainly.Api.Entities;

namespace Plainly.Api.Data.AppDatabase.Configuration;

public class UserConfiguration : EntityConfiguration<User>
{
    public override void Configure(EntityTypeBuilder<User> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.FullName)
            .IsRequired()
            .HasMaxLength(64);
    }
}