using Microsoft.EntityFrameworkCore;
using Plainly.Api.Infrastructure.Logging.Interfaces;
using Plainly.Api.Infrastructure.Logging.Loggers;

namespace Plainly.Api.Infrastructure.Logging.Providers;

public class DbLoggerProvider<TDbContext, TLogEntry>(IServiceProvider serviceProvider) : ILoggerProvider
    where TDbContext : DbContext, ILogDbContext<TLogEntry>
    where TLogEntry : class, ILogEntry, new()
{
    public ILogger CreateLogger(string categoryName) => new DbLogger<TDbContext, TLogEntry>(categoryName, serviceProvider);

    public void Dispose() { }
}