namespace Library.Domain.Entities
{
    public class Author:BaseEntity
    {
        public Author()
        {
            this.Books=new HashSet<Book>();
        }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public DateTime BirthDay { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
