using Microsoft.EntityFrameworkCore;
using Plainly.Api.Infrastructure.Logging.Interfaces;
using Plainly.Api.Models;

namespace Plainly.Api.Database;

public class LogDbContext(DbContextOptions<LogDbContext> options) : DbContext(options), ILogDbContext<LogEntry>
{
    public DbSet<LogEntry> Logs { get; set; }
}
