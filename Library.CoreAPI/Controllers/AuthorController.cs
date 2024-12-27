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
            if(response.IsSuccess)
            {
                return Ok(response);
            }
            return StatusCode(500, response.Message);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetAuthorByIdRequest(id));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            if (response.Message.Contains("Author with this id doesn't exist."))
            {
                return NotFound(response.Message); // 404
            }
            return StatusCode(500, response.Message);
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Post([FromBody] AuthorDTO newAuthor)
        {
            var response = await _mediator.Send(new AddAuthorRequest(newAuthor));
            if (response.IsSuccess)
            {
                var createdAuthor = (AuthorDTO)response.Result;

                return CreatedAtAction(nameof(Post), new { id = createdAuthor.Id }, createdAuthor); // 201
            }
            return StatusCode(500, response.Message);

        }

        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Put(int id, [FromBody] AuthorDTO author)
        {
            var response = await _mediator.Send(new UpdateAuthorRequest(id, author));
            if(response.IsSuccess)
            {
                return StatusCode(204,response.Message);
            }
            return StatusCode(500, response.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteAuthorRequest(id));
            if (response.IsSuccess)
            {
                return StatusCode(204, response.Message); // 204
            }
            if (response.Message.Contains("Author with this id doesn't exist."))
            {
                return NotFound(response.Message); // 404
            }
            return StatusCode(500, response.Message);
        }

        [HttpGet]
        [Route("{id:int}/books")]
        public async Task<IActionResult> GetBooksByAuthorId(int id)
        {
            var response = await _mediator.Send(new GetAuthorBooksRequest(id));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            if (response.Message.Contains("Author with this id doesn't exist."))
            {
                return NotFound(response.Message); // 404
            }
            return StatusCode(500, response.Message);
        }
    }
}
