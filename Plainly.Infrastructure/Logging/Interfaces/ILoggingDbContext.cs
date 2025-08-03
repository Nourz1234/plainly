using Microsoft.EntityFrameworkCore;
using Plainly.Domain.Interfaces;

namespace Plainly.Infrastructure.Logging.Interfaces;

public interface ILogDbContext<TLogEntry>
    where TLogEntry : class, ILogEntry
{
    DbSet<TLogEntry> Logs { get; }
}