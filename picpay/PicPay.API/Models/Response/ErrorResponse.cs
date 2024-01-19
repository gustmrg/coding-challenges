namespace PicPay.API.Models.Response;

public class ErrorResponse
{
    public string Code { get; set; }
    public string Message { get; set; }
    public string? Detail { get; set; }
    public string? TraceId { get; set; }
}