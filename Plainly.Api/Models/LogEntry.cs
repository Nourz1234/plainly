using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Plainly.Api.Infrastructure.Logging.Interfaces;

namespace Plainly.Api.Models;

public class LogEntry : ILogEntry
{
    public int Id { get; set; }
    [Required]
    [NotNull]
    public DateTime Timestamp { get; set; }
    [Required]
    [NotNull]
    public string? Level { get; set; }
    [Required]
    [NotNull]
    public string? Category { get; set; }
    [Required]
    [NotNull]
    public string? Message { get; set; }
    public string? Exception { get; set; }
    public string? TraceId { get; set; }
}