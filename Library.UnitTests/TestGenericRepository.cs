using Library.Domain.Entities;
using Library.Infrastructure.Data;
using Library.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.UnitTests
{
    public class TestGenericRepository
    {
        private DbContextOptions<AppDbContext> _options;

        public TestGenericRepository()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
        }

        private AppDbContext CreateContext()
        {
            var context = new AppDbContext(_options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task ListAllAsync_ShouldReturnListOfAuthors()
        {
            // Arrange
            using var context = CreateContext();
            SeedData(context);
            var repository = new Repository<Author>(context);

            // Act
            var result = await repository.ListAllAsync(CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Victor", result[0].Name);
            Assert.Equal("Melwill", result[1].LastName);
        }

        //[Fact]
        //public async Task DeleteAsync_ShouldReturnFalse_WhenEntityDoesNotExist()
        //{
        //    // Arrange
        //    using var context = CreateContext();
        //    SeedData(context);
        //    var repository = new Repository<Author>(context);

        //    // Act
        //    var result = await repository.DeleteAsync(99); // ID 99 не существует

        //    // Assert
        //    Assert.False(result);
        //    var allEntities = await repository.ListAllAsync(CancellationToken.None);
        //    Assert.Equal(2, allEntities.Count); // Убедиться, что начальные данные не изменились
        //}

        private void SeedData(AppDbContext context)
        {
            context.Authors.RemoveRange(context.Authors); // Очистить базу перед добавлением
            context.Authors.AddRange(
                new Author
                {
                    Name = "Victor",
                    LastName = "Hugo",
                    Country = "France",
                    BirthDay = new DateTime(1802, 5, 15),
                },
                new Author
                {
                    Name = "Herman",
                    LastName = "Melwill",
                    Country = "USA",
                    BirthDay = new DateTime(1819, 3, 22),
                }
            );
            context.SaveChanges();
            context.ChangeTracker.Clear();
        }
    }
}
  
