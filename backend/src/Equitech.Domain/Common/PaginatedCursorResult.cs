using System.Text.Json.Serialization;

namespace Equitech.Domain.Common;

public class PaginatedCursorResult<T>
{
    [JsonPropertyName("items")]
    public IReadOnlyList<T> Items { get; set; } = new List<T>();

    [JsonPropertyName("next_cursor")]
    public string? NextCursor { get; set; }

    [JsonPropertyName("has_more")]
    public bool HasMore { get; set; }

    [JsonPropertyName("limit")]
    public int Limit { get; set; }
}
