namespace Library.Application.DTOs
{
    public class ResponseData<T>
    {
        public T Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        
    }
}
