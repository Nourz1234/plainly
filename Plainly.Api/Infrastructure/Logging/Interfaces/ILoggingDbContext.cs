using Microsoft.EntityFrameworkCore;

namespace Plainly.Api.Infrastructure.Logging.Interfaces;

public interface ILoggingDbContext<TLogEntry>
    where TLogEntry : class, ILogEntry
{
    DbSet<TLogEntry> Logs { get; }
}