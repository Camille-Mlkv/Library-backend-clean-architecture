using AutoMapper;
using Library.Infrastructure.Identity;

namespace Library.Infrastructure.Services
{
    public class InfrastructureMappingProfile:Profile
    {
        public InfrastructureMappingProfile()
        {
            CreateMap<ApplicationUser, User>().ReverseMap();
        }
    }
}
