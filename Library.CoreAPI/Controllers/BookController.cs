using Library.Application.BookUseCases.Queries;
using Library.Application.DTOs;
using Library.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Library.CoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpGet("throw")]
        public IActionResult ThrowException()
        {
            throw new Exception("Проверка middleware.");
        }

        [HttpGet]
        public async Task<ResponseData> GetAll()
        {
            return await _mediator.Send(new GetAllBooksRequest());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ResponseData> Get(int id)
        {
            var response = await _mediator.Send(new GetBookByIdRequest(id));
            return response;
        }

        [HttpGet]
        [Route("paged")]
        public async Task<ResponseData> GetBooksPerPage(int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetBooksPerPageRequest(pageNo, pageSize));
            return response;
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Post([FromForm] BookDTO newBook)
        {
            var response = await _mediator.Send(new AddBookRequest(newBook));
            var createdBook = (BookDTO)response.Result;
            if (createdBook != null)
            {
                return CreatedAtAction(nameof(Post), new { id = createdBook.Id }, createdBook);
            }

            return BadRequest(response.Message);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<ResponseData> Put(int id, [FromForm] BookDTO updatedBook)
        {
            var response = await _mediator.Send(new UpdateBookRequest(id, updatedBook));
            return response;
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<ResponseData> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteBookRequest(id));
            return response;
        }

        [HttpGet]
        [Route("available")]
        public async Task<ResponseData> GetAvailableBooks(int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetAvailableBooksRequest(pageNo, pageSize));
            return response;
        }


        [HttpGet]
        [Route("filtered")]
        public async Task<ResponseData> GetBooks(string? genre = null, string? isbn = null, int? authorId = null, string? title = null, int pageNo = 1, int pageSize = 3)
        {
            if (!string.IsNullOrEmpty(genre))
            {
                return await _mediator.Send(new GetBooksByGenreRequest(genre, pageNo, pageSize));
            }
            else if (!string.IsNullOrEmpty(isbn))
            {
                return await _mediator.Send(new GetBooksByIsbnRequest(isbn, pageNo, pageSize));
            }
            else if (authorId.HasValue)
            {
                return await _mediator.Send(new GetBooksByAuthorRequest(authorId.Value, pageNo, pageSize));
            }
            else if (!string.IsNullOrEmpty(title))
            {
                return await _mediator.Send(new GetBooksByTitleRequest(title, pageNo, pageSize));
            }

            return new ResponseData();
        }

        [HttpGet]
        [Route("clientBooks")]
        [Authorize]
        public async Task<ResponseData> GetClientBooks(string clientId)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return await _mediator.Send(new GetClientBooksRequest(clientId,currentUserId));
        }

        [HttpPut]
        [Authorize]
        public async Task<ResponseData> AssignBookToClient(int bookId, string clientId)
        {
            return await _mediator.Send(new AssignBookToClientRequest(bookId, clientId));
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetBookImage(int id)
        {
            Response.Headers["Cache-Control"] = "public, max-age=86400"; // 1 day
            Response.Headers["Expires"] = DateTime.UtcNow.AddDays(1).ToString("R");

            var img =await _mediator.Send(new GetBookImageRequest(id));
            return File(img, "image/jpeg");
           
        }
    }
}
