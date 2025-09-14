using Inspection.Domain.Entities;

namespace Inspection.Application.Abstraction;

/// <summary>
/// Unit of Work interface for managing transactions and repository access
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Business repository
    /// </summary>
    IBusinessRepository Businesses { get; }

    /// <summary>
    /// Inspector repository
    /// </summary>
    IRepository<Inspector> Inspectors { get; }

    /// <summary>
    /// Inspection visit repository
    /// </summary>
    IInspectionVisitRepository InspectionVisits { get; }

    /// <summary>
    /// Inspection item repository
    /// </summary>
    IRepository<InspectionItem> InspectionItems { get; }

    /// <summary>
    /// Inspection result repository
    /// </summary>
    IRepository<InspectionResult> InspectionResults { get; }

    /// <summary>
    /// Saves all changes to the database
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of affected records</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a database transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Database transaction</returns>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task</returns>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
