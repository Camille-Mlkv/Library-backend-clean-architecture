using Library.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Library.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _entities;
        public Repository(AppDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();

        }

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
        {
            return await _entities.ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includesProperties)
        {
            IQueryable<T>? query = _entities.AsQueryable();
            if (includesProperties != null)
            {
                if (includesProperties.Any())
                {
                    foreach (Expression<Func<T, object>> included in includesProperties)
                    {
                        query = query.Include(included);
                    }
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync(cancellationToken);
        }


        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public Task Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return Task.CompletedTask;
        }

        public Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[]? includesProperties)
        {

            IQueryable<T> query = _entities.AsQueryable();
            query = query.AsNoTracking();

            query = query.Where(entity => entity.Id == id);
            
            if (includesProperties != null && includesProperties.Any())
            {
                foreach (var property in includesProperties)
                {
                    query = query.Include(property);
                }
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<T>> GetPagedListAsync(int pageNumber = 1, int pageSize = 3, CancellationToken cancellationToken = default, Expression<Func<T, bool>>? filter = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var items = await query
                .OrderBy(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return items;
        }

    }

}
