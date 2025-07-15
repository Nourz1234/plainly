using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Plainly.Api.Infrastructure.Logging.Interfaces;

namespace Plainly.Api.Infrastructure.Logging.Loggers;

public class DbLogger<TDbContext, TLogEntry> : ILogger
    where TDbContext : DbContext, ILoggingDbContext<TLogEntry>
    where TLogEntry : class, ILogEntry, new()
{
    private bool _IsRunning = false;
    private readonly string _CategoryName;
    private readonly IServiceProvider _ServiceProvider;

    public DbLogger(string categoryName, IServiceProvider serviceProvider)
    {
        _CategoryName = categoryName;
        _ServiceProvider = serviceProvider;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (_IsRunning) return;
        _IsRunning = true;

        var message = formatter(state, exception);

        // Create scope manually to resolve scoped DbContext
        using var scope = _ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        dbContext.Logs.Add(new TLogEntry()
        {
            Timestamp = DateTime.UtcNow,
            Level = logLevel.ToString(),
            Category = _CategoryName,
            Message = message,
            Exception = exception?.ToString(),
            TraceId = Activity.Current?.TraceId.ToString()
        });

        dbContext.SaveChanges();
        _IsRunning = false;
    }
}