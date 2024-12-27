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
        public async Task<IActionResult> GetAll()
        {
            var response= await _mediator.Send(new GetAllBooksRequest());
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return StatusCode(500, response.Message);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetBookByIdRequest(id));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            if (response.Message.Contains("Book with this id doesn't exist."))
            {
                return NotFound(response.Message); // 404
            }
            return StatusCode(500, response.Message);
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetBooksPerPage(int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetBooksPerPageRequest(pageNo, pageSize));
            if (response.IsSuccess)
            {
                return Ok(response);
            }

            if (response.Message.Contains("Provide valid data for page number and size"))
            {
                return BadRequest(response.Message);
            }
            return StatusCode(500, response.Message);
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Post([FromForm] BookDTO newBook)
        {
            var response = await _mediator.Send(new AddBookRequest(newBook));
            if(response.IsSuccess)
            {
                var createdBook = (BookDTO)response.Result;
                return CreatedAtAction(nameof(Post), new { id = createdBook.Id }, createdBook); // 201
            }

            if (response.Message.Contains("Book with this ISBN already exists.")) 
            {
                return Conflict(response.Message); // 409
            }

            return StatusCode(500, response.Message);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Put(int id, [FromForm] BookDTO updatedBook)
        {
            var response = await _mediator.Send(new UpdateBookRequest(id, updatedBook));
            if (response.IsSuccess)
            {
                return StatusCode(204,response.Message);
            }
            return StatusCode(500, response.Message);
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteBookRequest(id));
            if (response.IsSuccess)
            {
                return StatusCode(204, response.Message);
            }
            if (response.Message.Contains("Book with this id doesn't exist."))
            {
                return NotFound(response.Message);
            }

            return StatusCode(500, response.Message);
        }

        [HttpGet]
        [Route("available")]
        public async Task<IActionResult> GetAvailableBooks(int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetAvailableBooksRequest(pageNo, pageSize));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return StatusCode(500, response.Message);
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
        public async Task<IActionResult> GetClientBooks(string clientId)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var response= await _mediator.Send(new GetClientBooksRequest(clientId,currentUserId));
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            if(response.Message.Contains("Client can view only his own books"))
            {
                return Unauthorized(response.Message);
            }
            return StatusCode(500, response.Message);

        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AssignBookToClient(int bookId, string clientId)
        {
            var response= await _mediator.Send(new AssignBookToClientRequest(bookId, clientId));
            if(response.IsSuccess)
            {
                return StatusCode(204,response.Message);
            }
            if(response.Message.Contains("Book with this id doesn't exist."))
            {
                return NotFound(response.Message);
            }
            if (response.Message.Contains("Cannot assign the book as it's already assigned."))
            {
                return Conflict(response.Message);
            }
            if(response.Message.Contains("Client is invalid."))
            {
                return NotFound(response.Message);
            }
            return StatusCode(500, response.Message);
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
