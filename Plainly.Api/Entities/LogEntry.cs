using System.ComponentModel.DataAnnotations;
using Plainly.Api.Infrastructure.Logging.Interfaces;

namespace Plainly.Api.Entities;

public class LogEntry : ILogEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    [MaxLength(16)]
    public string Level { get; set; } = string.Empty;

    [Required]
    [MaxLength(256)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [MaxLength]
    public string Message { get; set; } = string.Empty;

    [MaxLength]
    public string? Exception { get; set; }

    [MaxLength(64)]
    public string? TraceId { get; set; }
}