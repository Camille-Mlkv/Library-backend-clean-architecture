using Azure;
using Library.Application.AuthorUseCases.Queries;
using Library.Application.BookUseCases.Commands;
using Library.Application.BookUseCases.Queries;
using Library.Application.DTOs;
using Library.BookAPI.Services;
using Library.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Library.BookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileService _fileService;
        public BookController(IMediator mediator,IFileService fileService)
        {
            _mediator = mediator;
            _fileService = fileService;
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
        public async Task<IActionResult> Post([FromForm] BookDTO newBook)
        {
            newBook.ImagePath=await _fileService.SaveFileAsync(newBook.ImageFile);
            var response = await _mediator.Send(new AddBookRequest(newBook));
            var createdBook = (BookDTO)response.Result;
            if (createdBook != null)
            {
                return CreatedAtAction(nameof(Post), new { id = createdBook.Id }, createdBook);
            }

            return BadRequest(response.Message);
        }

        [HttpPut("{id}")]
        public async Task<ResponseData> Put(int id, [FromForm] BookDTO updatedBook)
        {
            var existingBookResponse = await _mediator.Send(new GetBookByIdRequest(id));
            if (existingBookResponse.Result == null)
            {
                return existingBookResponse;
            }
            var existingBook = (BookDTO)existingBookResponse.Result;

            
            // Обработка изображения
            if (updatedBook.ImageFile != null)
            {
                if (existingBook.ImagePath != null)
                {
                    _fileService.DeleteFileAsync(existingBook.ImagePath);
                }

                updatedBook.ImagePath = await _fileService.SaveFileAsync(updatedBook.ImageFile);
            }
            else
            {
                updatedBook.ImagePath = "default-book.png";
            }

            var response = await _mediator.Send(new UpdateBookRequest(id, updatedBook));
            return response;
            
        }

        [HttpDelete("{id}")]
        public async Task<ResponseData> Delete(int id)
        {
            var existingBookResponse = await _mediator.Send(new GetBookByIdRequest(id));
            if (existingBookResponse.Result == null)
            {
                return existingBookResponse;
            }
            var existingBook = (BookDTO)existingBookResponse.Result;

            if (!string.IsNullOrEmpty(existingBook.ImagePath) && existingBook.ImagePath!= "default-book.png")
            {
                _fileService.DeleteFileAsync(existingBook.ImagePath);
            }

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
        public async Task<ResponseData> GetBooks(string? genre = null, string? isbn = null,int? authorId=null, string? title = null, int pageNo = 1, int pageSize = 3)
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
        [Route("myBooks")]
        public async Task<ResponseData> GetClientBooks(string clientId)
        {
            return await _mediator.Send(new GetClientBooksRequest(clientId));
        }

        [HttpPut]
        public async Task<ResponseData> AssignBookToClient(int bookId, string clientId)
        {
            if (bookId <= 0 || string.IsNullOrEmpty(clientId))
            {
                return new ResponseData
                {
                    IsSuccess = false,
                    Message = "Invalid book ID or client ID."
                };
            }
            return await _mediator.Send(new AssignBookToClientRequest(bookId, clientId));
        }

        [HttpGet("{id}/image")]
        public async Task<IActionResult> GetBookImage(int id)
        {
            var existingBookResponse = await _mediator.Send(new GetBookByIdRequest(id));
            if (existingBookResponse.Result == null)
            {
                return BadRequest(existingBookResponse.Message);
            }
            var existingBook = (BookDTO)existingBookResponse.Result;
            var imagePath = Path.Combine("wwwroot", "Images", $"{existingBook.ImagePath}");

            if (!System.IO.File.Exists(imagePath))
            {
                return NotFound("Image not found");
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(imagePath);

            Response.Headers.Add("Cache-Control", "public, max-age=86400"); // Кэшировать на 1 день
            Response.Headers.Add("Expires", DateTime.UtcNow.AddDays(1).ToString("R")); // Установка даты истечения кэша

            return File(fileBytes, "image/jpeg");
        }


    }
}
