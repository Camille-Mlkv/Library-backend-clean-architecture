using Library.Application.AuthorUseCases.Queries;
using Library.Application.DTOs;
using Library.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response= await _mediator.Send(new GetAllAuthorsRequest());
            return StatusCode(response.StatusCode,response.Result);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetAuthorByIdRequest(id));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Post([FromBody] AuthorDTO newAuthor)
        {
            var response = await _mediator.Send(new AddAuthorRequest(newAuthor));
            if (response.IsSuccess)
            {
                return CreatedAtAction(nameof(Post), new { id = response.Result.Id }, response.Result); // 201
            }
            return StatusCode(response.StatusCode, response.Message);

        }

        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Put(int id, [FromBody] AuthorDTO author)
        {
            var response = await _mediator.Send(new UpdateAuthorRequest(id, author));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteAuthorRequest(id));
            return StatusCode(response.StatusCode, response.Message);
        }

        [HttpGet]
        [Route("{id:int}/books")]
        public async Task<IActionResult> GetBooksByAuthorId(int id)
        {
            var response = await _mediator.Send(new GetAuthorBooksRequest(id));
            return StatusCode(response.StatusCode, response);
        }
    }
}
