using Library.Infrastructure.Data;

namespace Library.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IUserRepository _userRepository;

        public UnitOfWork(AppDbContext context, IRepository<Book> bookRepository, IRepository<Author> authorRepository,IUserRepository userRepository)
        {
            _context = context;
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _userRepository = userRepository;
        }
        public IRepository<Book> BookRepository => _bookRepository;
        public IRepository<Author> AuthorRepository => _authorRepository;
        public IUserRepository UserRepository => _userRepository;
        public async Task SaveAllAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
    }
}
