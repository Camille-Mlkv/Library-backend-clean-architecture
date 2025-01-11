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
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDTO model, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new UserRegistrationRequest(model),cancellationToken);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new UserLogInRequest(model),cancellationToken);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshModelDTO model, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new RefreshAccessTokenRequest(model),cancellationToken);
            return Ok(response);
        }

        [HttpDelete("revoke")]
        public async Task<IActionResult> RevokeRefreshToken(string username, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new RevokeRefreshTokenRequest(username),cancellationToken);
            return StatusCode(204,response.Message);
        }
    }
}
