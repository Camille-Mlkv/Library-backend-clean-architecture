using Library.Domain.Entities;
using Library.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Abstractions
{
    public interface IUserRepository
    {
        Task<string> Register(RegistrationRequestDTO registrationRequestDTO);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<LoginResponseDTO> RefreshAccessToken(RefreshModel refreshModel);

        Task<ResponseData> RevokeRefreshToken(string username);
    }
}
