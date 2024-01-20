namespace PicPay.API.Models.Response;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public List<Error> Errors { get; set; } = new();
    public string? Detail { get; set; }
    public string? TraceId { get; set; }
}

public record Error(string Message);