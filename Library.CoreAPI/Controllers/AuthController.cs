using Library.Application.AuthenticationUseCases.Queries;
using Library.Application.DTOs.Identity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Library.CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model)
        {
            var response = await _mediator.Send(new UserRegistrationRequest(model));
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var response = await _mediator.Send(new UserLogInRequest(model));
            return StatusCode(500, response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshModelDTO model)
        {
            var response = await _mediator.Send(new RefreshAccessTokenRequest(model));
            return StatusCode(response.StatusCode, response); 
        }

        [HttpDelete("revoke")]
        public async Task<IActionResult> RevokeRefreshToken(string username)
        {
            var response = await _mediator.Send(new RevokeRefreshTokenRequest(username));
            return StatusCode(response.StatusCode, response.Message);
        }
    }
}
