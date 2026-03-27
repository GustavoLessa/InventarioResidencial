using System.Text.Json;

namespace Inventario.API.Models;

public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<string>? Errors { get; set; } // Para listar múltiplos erros do FluentValidation

    public override string ToString() => JsonSerializer.Serialize(this);
}