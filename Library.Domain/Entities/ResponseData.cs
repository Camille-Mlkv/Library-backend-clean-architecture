namespace Library.Domain.Entities
{
    public class ResponseData<T>
    {
        public T Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        public int StatusCode { get; set; }
        
    }
}
