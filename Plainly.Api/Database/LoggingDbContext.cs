using Microsoft.EntityFrameworkCore;
using Plainly.Api.Infrastructure.Logging.Interfaces;
using Plainly.Api.Models;

namespace Plainly.Api.Database;

public class LoggingDbContext(DbContextOptions<LoggingDbContext> options) : DbContext(options), ILoggingDbContext<LogEntry>
{
    public DbSet<LogEntry> Logs { get; set; }
}
