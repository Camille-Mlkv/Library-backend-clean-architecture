using Library.Application.BookUseCases.Queries;
using Library.Application.DTOs;
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
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var response= await _mediator.Send(new GetAllBooksRequest(),cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetBookByIdRequest(id), cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetBooksPerPage(int pageNo = 1, int pageSize = 3, CancellationToken cancellationToken=default)
        {
            var response = await _mediator.Send(new GetBooksPerPageRequest(pageNo, pageSize), cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Post([FromForm] BookDTO newBook, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new AddBookRequest(newBook),cancellationToken);
            var createdBook = response.Result;
            return CreatedAtAction(nameof(Post), new { id = createdBook.Id }, createdBook); 
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Put(int id, [FromForm] BookDTO updatedBook, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new UpdateBookRequest(id, updatedBook),cancellationToken);
            return StatusCode(204,response.Message);
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new DeleteBookRequest(id),cancellationToken);
            return StatusCode(204, response.Message);
        }

        [HttpGet]
        [Route("available")]
        public async Task<IActionResult> GetAvailableBooks(int pageNo = 1, int pageSize = 3, CancellationToken cancellationToken=default)
        {
            var response = await _mediator.Send(new GetAvailableBooksRequest(pageNo, pageSize),cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("genre")]
        public async Task<IActionResult> GetBooksByGenre(string genre, int pageNo = 1, int pageSize = 3, CancellationToken cancellationToken=default)
        {
            var response = await _mediator.Send(new GetBooksByGenreRequest(genre, pageNo, pageSize),cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("isbn")]
        public async Task<IActionResult> GetBookByIsbn(string isbn, int pageNo = 1, int pageSize = 3, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetBookByIsbnRequest(isbn, pageNo, pageSize), cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("author")]
        public async Task<IActionResult> GetBooksByAuthor(int authorId, int pageNo = 1, int pageSize = 3, CancellationToken cancellationToken = default)
        {
            var response = await _mediator.Send(new GetBooksByAuthorRequest(authorId, pageNo, pageSize),cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("title")]
        public async Task<IActionResult> GetBooksByTilte(string title, int pageNo = 1, int pageSize = 3, CancellationToken cancellationToken=default)
        {
            var response = await _mediator.Send(new GetBooksByTitleRequest(title, pageNo, pageSize),cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        [Route("clientBooks")]
        [Authorize]
        public async Task<IActionResult> GetClientBooks(string clientId, CancellationToken cancellationToken = default)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var response= await _mediator.Send(new GetClientBooksRequest(clientId,currentUserId),cancellationToken);
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AssignBookToClient(int bookId, string clientId, CancellationToken cancellationToken = default)
        {
            var response= await _mediator.Send(new AssignBookToClientRequest(bookId, clientId),cancellationToken);
            return StatusCode(204, response.Message);
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetBookImage(int id, CancellationToken cancellationToken = default)
        {
            Response.Headers["Cache-Control"] = "public, max-age=86400"; // 1 day
            Response.Headers["Expires"] = DateTime.UtcNow.AddDays(1).ToString("R");

            var img =await _mediator.Send(new GetBookImageRequest(id),cancellationToken);
            return File(img, "image/jpeg");
           
        }
    }
}
