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
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            if (response.Message.Contains("An error occured while registrating the user"))
            {
                return BadRequest(response.Message); // 400
            }

            return StatusCode(500, response.Message);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var response = await _mediator.Send(new UserLogInRequest(model));
            if (response.IsSuccess)
            {
                return Ok(response); //200
            }
            if (response.Message.Contains("Wrong credentials"))
            {
                return Unauthorized(response.Message); // 401
            }

            return StatusCode(500, response.Message);
        }

        // Refresh access token
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshAccessToken([FromBody] RefreshModelDTO model)
        {
            var response = await _mediator.Send(new RefreshAccessTokenRequest(model));
            if (response.IsSuccess)
            {
                return Ok(response.Result);  //200
            }

            if (response.Message.Contains("Access token not refreshed"))
            {
                return Unauthorized(response.Message); // 401
            }

            if (response.Message.Contains("validation") || response.Message.Contains("invalid"))
            {
                return BadRequest(response.Message); // 400 
            }

            return StatusCode(500, response.Message); // Internal Server Error
        }

        // Revoke
        [HttpDelete("revoke")]
        public async Task<IActionResult> RevokeRefreshToken(string username)
        {
            var response = await _mediator.Send(new RevokeRefreshTokenRequest(username));
            if (response.IsSuccess)
            {
                return StatusCode(204, response.Message);
            }

            if (response.Message.Contains("User doesn't exist"))
            {
                return BadRequest(response.Message); // 400
            }
            return StatusCode(500, response.Message);
        }
    }
}
