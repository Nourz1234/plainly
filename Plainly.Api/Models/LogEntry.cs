using System.ComponentModel.DataAnnotations;
using Plainly.Api.Infrastructure.Logging.Interfaces;

namespace Plainly.Api.Models;

public class LogEntry : ILogEntry
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    [Required]
    [StringLength(16)]
    public string Level { get; set; } = string.Empty;

    [Required]
    [StringLength(256)]
    public string Category { get; set; } = string.Empty;

    [Required]
    [MaxLength]
    public string Message { get; set; } = string.Empty;

    [MaxLength]
    public string? Exception { get; set; }

    [StringLength(64)]
    public string? TraceId { get; set; }
}