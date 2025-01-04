using AutoMapper;
using Library.Infrastructure.Identity;

namespace Library.Infrastructure.MappingProfiles
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, User>().ReverseMap();
        }
    }
}
