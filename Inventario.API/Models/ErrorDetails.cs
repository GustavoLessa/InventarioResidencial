using System.Text.Json;

namespace Inventario.API.Models;

public class ErrorDetails
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string>? Errors { get; set; }

    public override string ToString() => JsonSerializer.Serialize(this, JsonOptions);
}
