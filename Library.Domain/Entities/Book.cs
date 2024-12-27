using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Library.Domain.Entities
{
    public class Book:BaseEntity
    {
        public string ISBN {  get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Description { get; set; }
        //[ForeignKey("AuthorId")]
        public int AuthorId { get; set; }
        //[JsonIgnore]
        public virtual Author Author { get; set; }
        //[ForeignKey("ClientId")]
        public string? ClientId { get; set; }
        public DateTime? TakenTime { get; set; }
        public DateTime? ReturnBy { get; set; }
        public string? ImagePath { get; set; }
    }
}
