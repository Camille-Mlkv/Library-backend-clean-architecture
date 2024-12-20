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
        public async Task<ResponseData> GetAll()
        {
            return await _mediator.Send(new GetAllAuthorsRequest());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponseData> Get(int id)
        {
            var response = await _mediator.Send(new GetAuthorByIdRequest(id));
            return response;
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Post([FromBody] AuthorDTO newAuthor)
        {
            var response = await _mediator.Send(new AddAuthorRequest(newAuthor));
            var createdAuthor = (AuthorDTO)response.Result;
            //int authorId = createdAuthor.Id;

            return CreatedAtAction(nameof(Post), new { id = createdAuthor.Id }, createdAuthor);

        }

        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<ResponseData> Put(int id, [FromBody] AuthorDTO author)
        {
            var response = await _mediator.Send(new UpdateAuthorRequest(id, author));
            return response;
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<ResponseData> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteAuthorRequest(id));
            return response;
        }

        [HttpGet]
        [Route("{id:int}/books")]
        public async Task<ResponseData> GetBooksByAuthorId(int id)
        {
            var response = await _mediator.Send(new GetAuthorBooksRequest(id));
            return response;
        }
    }
}
