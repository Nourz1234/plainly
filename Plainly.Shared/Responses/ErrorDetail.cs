using System.Text.Json.Serialization;

namespace Plainly.Shared.Responses;

public record ErrorDetail(
    string Code,
    string Description,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Field = null
);
