using Microsoft.EntityFrameworkCore;
using Plainly.Api.Infrastructure.Logging.Interfaces;
using Plainly.Api.Infrastructure.Logging.Loggers;

namespace Plainly.Api.Infrastructure.Logging.Providers;

public class DbLoggerProvider<TDbContext, TLogEntry> : ILoggerProvider
    where TDbContext : DbContext, ILoggingDbContext<TLogEntry>
    where TLogEntry : class, ILogEntry, new()
{
    private readonly IServiceProvider _serviceProvider;

    public DbLoggerProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ILogger CreateLogger(string categoryName)
        => new DbLogger<TDbContext, TLogEntry>(categoryName, _serviceProvider);

    public void Dispose() { }
}