using Library.Application.AuthorUseCases.Queries;
using Library.Application.DTOs;
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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var response= await _mediator.Send(new GetAllAuthorsRequest(),cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id,CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetAuthorByIdRequest(id),cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Post([FromBody] AuthorDTO newAuthor,CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new AddAuthorRequest(newAuthor), cancellationToken);
            return CreatedAtAction(nameof(Post), new { id = response.Result.Id }, response.Result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Put(int id, [FromBody] AuthorDTO author, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new UpdateAuthorRequest(id, author), cancellationToken);
            return StatusCode(204, response.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new DeleteAuthorRequest(id),cancellationToken);
            return StatusCode(204, response.Message);
        }

        [HttpGet]
        [Route("{id:int}/books")]
        public async Task<IActionResult> GetBooksByAuthorId(int id, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetAuthorBooksRequest(id),cancellationToken);
            return Ok(response);
        }
    }
}
