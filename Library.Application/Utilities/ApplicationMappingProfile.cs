using Library.Application.DTOs.Identity;

namespace Library.Application.Utilities
{
    public class ApplicationMappingProfile:Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<AuthorDTO, Author>().ReverseMap();
            CreateMap<BookDTO,Book>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
        

    }
}
