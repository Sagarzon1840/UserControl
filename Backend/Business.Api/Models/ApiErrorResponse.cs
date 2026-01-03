namespace CreditAppManager.Api.Models;

public class ApiErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string? Instance { get; set; }
    public string? TraceId { get; set; }
    public Dictionary<string, string[]>? Errors { get; set; }

    public static ApiErrorResponse ValidationError(
        Dictionary<string, string[]> errors,
        string instance,
        string traceId)
    {
        return new ApiErrorResponse
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Uno o más errores de validación ocurrieron.",
            Status = 400,
            Instance = instance,
            TraceId = traceId,
            Errors = errors
        };
    }
}
