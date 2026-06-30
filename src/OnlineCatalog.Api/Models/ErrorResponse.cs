namespace OnlineCatalog.Api.Models;

public class ErrorResponse
{
    public int Status { get; init; }
    public string Message { get; init; } = default!;
    public string? TraceId { get; init; }
    public Dictionary<string, string[]>? Errors { get; init; }
}
