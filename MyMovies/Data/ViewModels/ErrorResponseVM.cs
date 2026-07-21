namespace MyMovies.Data.ViewModels;

public class ErrorResponseVM
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}