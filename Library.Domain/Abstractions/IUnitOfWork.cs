using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        IRepository<Book> BookRepository { get; }
        IRepository<Author> AuthorRepository { get; }
        IUserRepository UserRepository { get; }
        public Task SaveAllAsync();
        public Task DeleteDataBaseAsync();
        public Task CreateDataBaseAsync();
    }
}
