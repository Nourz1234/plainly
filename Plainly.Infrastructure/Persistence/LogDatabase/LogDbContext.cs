using Microsoft.EntityFrameworkCore;
using Plainly.Domain.Entities;
using Plainly.Infrastructure.Logging.Interfaces;

namespace Plainly.Infrastructure.Persistence.LogDatabase;

public class LogDbContext(DbContextOptions<LogDbContext> options) : DbContext(options), ILogDbContext<LogEntry>
{
    public DbSet<LogEntry> Logs { get; set; }
}
