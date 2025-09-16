using Inspection.Domain.Entities;

namespace Inspection.DataAccessLayer.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Inspector> Inspectors { get; }
        IGenericRepository<EntityToInspect> EntitiesToInspect { get; }
        IGenericRepository<InspectionVisit> InspectionVisits { get; }
        IGenericRepository<Violation> Violations { get; }
        IGenericRepository<ApplicationUser> applicationUser { get; }
        IGenericRepository<ApplicationRole> applicationRole { get; }
        Task<int> SaveChangesAsync();
        int SaveChanges();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
