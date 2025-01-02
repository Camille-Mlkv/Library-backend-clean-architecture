namespace Library.Application.Utilities
{
    public class CustomHttpException:Exception
    {
        public int StatusCode { get; }
        public string? Details { get; }

        public CustomHttpException(int statusCode, string message, string? details = null): base(message)
        {
            StatusCode = statusCode;
            Details = details;
        }
    }
}
