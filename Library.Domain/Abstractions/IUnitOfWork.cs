using Library.Domain.Entities;

namespace Library.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<Book> BookRepository { get; }
        IRepository<Author> AuthorRepository { get; }
        IUserRepository UserRepository { get; }
        public Task SaveAllAsync();
    }
}
