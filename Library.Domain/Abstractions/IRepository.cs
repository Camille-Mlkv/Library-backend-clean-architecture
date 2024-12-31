using System.Linq.Expressions;

namespace Library.Domain.Abstractions
{
    public interface IRepository<T> where T: class
    {
        /// <summary>
        /// Получение всего списка сущностей.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Получение отфильтрованного списка.
        /// </summary>
        /// <param name="filter">Делегат-условие отбора</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includesProperties">Делегаты для подключения навигационных свойств</param>
        /// <returns></returns>
        Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[]? includesProperties);

        /// <summary>
        /// Добавление новой сущности.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Изменение сущности.
        /// </summary>
        /// <param name="entity">Сущность с измененным содержимым</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаление сущности.
        /// </summary>
        /// <param name="entity">Сущность, которую следует удалить</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DeleteAsync(T entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Поиск сущности по Id.
        /// </summary>
        /// <param name="id">Id сущности</param>
        /// <param name="cancellationToken"></param>
        /// <param name="includesProperties">Делегаты для подключения навигационных свойств</param>
        /// <returns></returns>
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default,
            params Expression<Func<T, object>>[]? includesProperties);


        /// <summary>
        /// Получение списка сущностей с пагинацией.
        /// </summary>
        /// <param name="pageNumber">Номер страницы (начинается с 1)</param>
        /// <param name="pageSize">Количество элементов на странице</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Отфильтрованный список сущностей с учетом пагинации</returns>
        Task<List<T>> GetPagedListAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default, Expression<Func<T, bool>>? filter = null);

    }
}
