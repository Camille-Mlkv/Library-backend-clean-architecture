using Library.Application.DTOs.Identity;

namespace Library.Application.MappingProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
