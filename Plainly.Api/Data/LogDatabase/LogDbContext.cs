using Microsoft.EntityFrameworkCore;
using Plainly.Api.Entities;
using Plainly.Api.Infrastructure.Logging.Interfaces;

namespace Plainly.Api.Data.LogDatabase;

public class LogDbContext(DbContextOptions<LogDbContext> options) : DbContext(options), ILogDbContext<LogEntry>
{
    public DbSet<LogEntry> Logs { get; set; }
}
