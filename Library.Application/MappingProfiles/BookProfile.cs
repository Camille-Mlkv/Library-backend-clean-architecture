namespace Library.Application.MappingProfiles
{
    public class BookProfile:Profile
    {
        public BookProfile()
        {
            CreateMap<BookDTO, Book>().ReverseMap();
        }
    }
}
