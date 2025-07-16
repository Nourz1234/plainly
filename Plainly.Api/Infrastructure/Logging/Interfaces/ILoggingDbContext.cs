using Microsoft.EntityFrameworkCore;

namespace Plainly.Api.Infrastructure.Logging.Interfaces;

public interface ILogDbContext<TLogEntry>
    where TLogEntry : class, ILogEntry
{
    DbSet<TLogEntry> Logs { get; }
}