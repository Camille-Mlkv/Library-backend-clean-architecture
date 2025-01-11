namespace Library.Application.Exceptions
{
    public class CustomException:Exception
    {
        public string? Details { get; }
        public CustomException(string message, string? details = null) : base(message)
        {
            Details = details;
        }
    }
}
