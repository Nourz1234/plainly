using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Plainly.Api.Models;

namespace Plainly.Api.Data.AppDatabase.Configuration;

// public class UserConfiguration : IEntityTypeConfiguration<User>
// {
//     public void Configure(EntityTypeBuilder<User> builder)
//     {
//         builder.HasKey(m => m.Id);
//         builder.Property(m => m.CreatedAt)
//                 .IsRequired()
//                 .HasColumnType("Date")
//                 .HasDefaultValueSql("GetDate()");
//     }
// }