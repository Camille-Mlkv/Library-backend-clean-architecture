using Library.Application.AuthenticationUseCases.Queries;
using Library.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.AuthAPI.Controllers
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
            if(response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var response = await _mediator.Send(new UserLogInRequest(model));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        // Refresh access token
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshModel model)
        {
            var response = await _mediator.Send(new RefreshAccessTokenRequest(model));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        // Revoke
        [HttpDelete("revoke")]
        public async Task<IActionResult> RevokeRefreshToken(string username)
        {
            var response=await _mediator.Send(new RevokeRefreshTokenRequest(username));
            return Ok(response);
        }
    }
}
