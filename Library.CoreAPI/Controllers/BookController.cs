﻿using Library.Application.BookUseCases.Queries;
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
        public async Task<IActionResult> GetAll()
        {
            var response= await _mediator.Send(new GetAllBooksRequest());
            return Ok(response);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _mediator.Send(new GetBookByIdRequest(id));
            return Ok(response);
        }

        [HttpGet]
        [Route("paged")]
        public async Task<IActionResult> GetBooksPerPage(int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetBooksPerPageRequest(pageNo, pageSize));
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Post([FromForm] BookDTO newBook)
        {
            var response = await _mediator.Send(new AddBookRequest(newBook));
            var createdBook = response.Result;
            return CreatedAtAction(nameof(Post), new { id = createdBook.Id }, createdBook); 
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Put(int id, [FromForm] BookDTO updatedBook)
        {
            var response = await _mediator.Send(new UpdateBookRequest(id, updatedBook));
            return StatusCode(204,response.Message);
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteBookRequest(id));
            return StatusCode(204, response.Message);
        }

        [HttpGet]
        [Route("available")]
        public async Task<IActionResult> GetAvailableBooks(int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetAvailableBooksRequest(pageNo, pageSize));
            return Ok(response);
        }

        [HttpGet]
        [Route("genre")]
        public async Task<IActionResult> GetBooksByGenre(string genre, int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetBooksByGenreRequest(genre, pageNo, pageSize));
            return Ok(response);
        }

        [HttpGet]
        [Route("isbn")]
        public async Task<IActionResult> GetBookByIsbn(string isbn, int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetBookByIsbnRequest(isbn, pageNo, pageSize));
            return Ok(response);
        }

        [HttpGet]
        [Route("author")]
        public async Task<IActionResult> GetBooksByAuthor(int authorId, int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetBooksByAuthorRequest(authorId, pageNo, pageSize));
            return Ok(response);
        }

        [HttpGet]
        [Route("title")]
        public async Task<IActionResult> GetBooksByTilte(string title, int pageNo = 1, int pageSize = 3)
        {
            var response = await _mediator.Send(new GetBooksByTitleRequest(title, pageNo, pageSize));
            return Ok(response);
        }

        [HttpGet]
        [Route("clientBooks")]
        [Authorize]
        public async Task<IActionResult> GetClientBooks(string clientId)
        {
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var response= await _mediator.Send(new GetClientBooksRequest(clientId,currentUserId));
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> AssignBookToClient(int bookId, string clientId)
        {
            var response= await _mediator.Send(new AssignBookToClientRequest(bookId, clientId));
            return StatusCode(204, response.Message);
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
