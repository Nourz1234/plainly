using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Plainly.Domain.Interfaces;
using Plainly.Infrastructure.Logging.Interfaces;

namespace Plainly.Infrastructure.Logging.Loggers;

public class DbLogger<TDbContext, TLogEntry>(string categoryName, IServiceProvider serviceProvider) : ILogger
    where TDbContext : DbContext, ILogDbContext<TLogEntry>
    where TLogEntry : class, ILogEntry, new()
{
    private static bool _IsSaving = false;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => !(_IsSaving && categoryName.StartsWith("Microsoft.EntityFrameworkCore"));

    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        var message = formatter(state, exception);

        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        dbContext.Logs.Add(new TLogEntry()
        {
            Timestamp = DateTime.UtcNow,
            Level = logLevel.ToString(),
            Category = categoryName,
            Message = message,
            Exception = exception?.ToString(),
            TraceId = Activity.Current?.TraceId.ToString()
        });

        _IsSaving = true;
        dbContext.SaveChanges();
        _IsSaving = false;
    }
}