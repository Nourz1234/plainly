using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Plainly.Domain.Interfaces;
using Plainly.Infrastructure.Logging.Interfaces;
using Plainly.Infrastructure.Logging.Loggers;

namespace Plainly.Infrastructure.Logging.Providers;

public class DbLoggerProvider<TDbContext, TLogEntry>(IServiceProvider serviceProvider) : ILoggerProvider
    where TDbContext : DbContext, ILogDbContext<TLogEntry>
    where TLogEntry : class, ILogEntry, new()
{
    public ILogger CreateLogger(string categoryName) => new DbLogger<TDbContext, TLogEntry>(categoryName, serviceProvider);

    public void Dispose() { }
}