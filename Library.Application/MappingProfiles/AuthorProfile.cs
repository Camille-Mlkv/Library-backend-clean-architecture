namespace Library.Application.MappingProfiles
{
    public class AuthorProfile:Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorDTO, Author>().ReverseMap();
        }
    }
}
