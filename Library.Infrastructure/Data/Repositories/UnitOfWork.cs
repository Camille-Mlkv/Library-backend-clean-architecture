using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly Lazy<IRepository<Book>> _bookRepository;
        private readonly Lazy<IRepository<Author>> _authorRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            _bookRepository = new Lazy<IRepository<Book>>(() => new Repository<Book>(context));
            _authorRepository = new Lazy<IRepository<Author>>(() => new Repository<Author>(context));
        }
        public IRepository<Book> BookRepository => _bookRepository.Value;
        public IRepository<Author> AuthorRepository => _authorRepository.Value;

        public async Task CreateDataBaseAsync() => await _context.Database.EnsureCreatedAsync();
        public async Task DeleteDataBaseAsync() => await _context.Database.EnsureDeletedAsync();
        public async Task SaveAllAsync() => await _context.SaveChangesAsync();
    }
}
