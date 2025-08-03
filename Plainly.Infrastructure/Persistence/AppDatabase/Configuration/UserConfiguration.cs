using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plainly.Infrastructure.Persistence.AppDatabase.Entities;

namespace Plainly.Infrastructure.Persistence.AppDatabase.Configuration;

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