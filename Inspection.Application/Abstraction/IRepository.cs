using System.Linq.Expressions;

namespace Inspection.Application.Abstraction;

/// <summary>
/// Generic repository interface for common data operations
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gets an entity by its ID
    /// </summary>
    /// <param name="id">Entity ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Entity or null if not found</returns>
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of entities</returns>
    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Finds entities based on a predicate
    /// </summary>
    /// <param name="predicate">Search predicate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of matching entities</returns>
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the first entity matching the predicate or null
    /// </summary>
    /// <param name="predicate">Search predicate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>First matching entity or null</returns>
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paged list of entities
    /// </summary>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of entities</returns>
    Task<(List<T> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paged list of entities with filtering
    /// </summary>
    /// <param name="predicate">Filter predicate</param>
    /// <param name="pageNumber">Page number (1-based)</param>
    /// <param name="pageSize">Number of items per page</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of entities</returns>
    Task<(List<T> Items, int TotalCount)> GetPagedAsync(Expression<Func<T, bool>> predicate, int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new entity
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Added entity</returns>
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds multiple entities
    /// </summary>
    /// <param name="entities">Entities to add</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an entity
    /// </summary>
    /// <param name="entity">Entity to update</param>
    void Update(T entity);

    /// <summary>
    /// Updates multiple entities
    /// </summary>
    /// <param name="entities">Entities to update</param>
    void UpdateRange(IEnumerable<T> entities);

    /// <summary>
    /// Removes an entity
    /// </summary>
    /// <param name="entity">Entity to remove</param>
    void Remove(T entity);

    /// <summary>
    /// Removes multiple entities
    /// </summary>
    /// <param name="entities">Entities to remove</param>
    void RemoveRange(IEnumerable<T> entities);

    /// <summary>
    /// Checks if any entity matches the predicate
    /// </summary>
    /// <param name="predicate">Search predicate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if any entity matches</returns>
    Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Counts entities matching the predicate
    /// </summary>
    /// <param name="predicate">Search predicate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Count of matching entities</returns>
    Task<int> CountAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
}
