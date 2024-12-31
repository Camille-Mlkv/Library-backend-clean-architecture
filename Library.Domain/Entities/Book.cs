namespace Library.Domain.Entities
{
    public class Book:BaseEntity
    {
        public string ISBN {  get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }
        public string? ClientId { get; set; }
        public DateTime? TakenTime { get; set; }
        public DateTime? ReturnBy { get; set; }
        public string? ImagePath { get; set; }
    }
}
