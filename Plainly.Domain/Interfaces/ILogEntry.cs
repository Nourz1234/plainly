namespace Plainly.Domain.Interfaces;

public interface ILogEntry
{
    int Id { get; set; }
    DateTime Timestamp { get; set; }
    string Level { get; set; }
    string Category { get; set; }
    string Message { get; set; }
    string? Exception { get; set; }
    string? TraceId { get; set; }
}