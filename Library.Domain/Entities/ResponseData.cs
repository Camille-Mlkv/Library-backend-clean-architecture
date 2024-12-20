namespace Library.Domain.Entities
{
    public class ResponseData
    {
        public object Result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = "";
        
    }
}
