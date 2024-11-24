using AutoMapper;
using Library.Application.AuthorUseCases.Commands;
using Library.Application.AuthorUseCases.Queries;
using Library.Application.DTOs;
using Library.Domain.Abstractions;
using Library.Domain.Entities;
using Moq;
using System.Linq.Expressions;

namespace Library.UnitTests
{
    public class AddAuthorRequestHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;

        public AddAuthorRequestHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenAuthorIsAddedSuccessfully()
        {
            // Arrange
            AddAuthorRequestHandler _handler= new AddAuthorRequestHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var authorDto = new AuthorDTO { Name = "Victor", LastName = "Hugo", Country = "France", BirthDay = new DateTime(1802, 5, 15) };
            var request = new AddAuthorRequest(authorDto);
            var author = new Author { Name = "Victor", LastName = "Hugo", Country = "France", BirthDay = new DateTime(1802, 5, 15) };

            _mockMapper.Setup(m => m.Map<Author>(It.IsAny<AuthorDTO>())).Returns(author);

            _mockUnitOfWork.Setup(u => u.AuthorRepository.AddAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Author added successfully.", result.Message);
            _mockUnitOfWork.Verify(u => u.AuthorRepository.AddAsync(It.IsAny<Author>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllAuthors_WhenAuthorsExist()
        {
            // Arrange
            GetAllAuthorsRequestHandler _handler = new GetAllAuthorsRequestHandler(_mockUnitOfWork.Object, _mockMapper.Object);
            var authors = new List<Author>
            {
                new Author { Name = "Victor", LastName = "Hugo", Country = "France", BirthDay = new DateTime(1802, 5, 15) },
                new Author { Name = "Herman", LastName = "Melville", Country = "USA", BirthDay = new DateTime(1819, 8, 1) }
            };

            var authorDtos = new List<AuthorDTO>
            {
                new AuthorDTO { Name = "Victor", LastName = "Hugo", Country = "France", BirthDay = new DateTime(1802, 5, 15) },
                new AuthorDTO { Name = "Herman", LastName = "Melville", Country = "USA", BirthDay = new DateTime(1819, 8, 1) }
            };

            _mockUnitOfWork.Setup(u => u.AuthorRepository.ListAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(authors);

            _mockMapper.Setup(m => m.Map<List<AuthorDTO>>(authors)).Returns(authorDtos);

            var request = new GetAllAuthorsRequest();

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal("All authors are retrieved.", response.Message);
            Assert.NotNull(response.Result);
            Assert.Equal(2, ((List<AuthorDTO>)response.Result).Count);
            Assert.Equal("Victor", ((List<AuthorDTO>)response.Result)[0].Name);
            Assert.Equal("Herman", ((List<AuthorDTO>)response.Result)[1].Name);

            _mockUnitOfWork.Verify(u => u.AuthorRepository.ListAllAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockMapper.Verify(m => m.Map<List<AuthorDTO>>(authors), Times.Once);
        }

    }
}