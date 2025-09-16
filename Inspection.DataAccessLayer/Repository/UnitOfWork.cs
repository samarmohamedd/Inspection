using Microsoft.EntityFrameworkCore.Storage;
using Inspection.DataAccessLayer.Context;
using Inspection.Domain.Entities;

namespace Inspection.DataAccessLayer.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InspectionDbContext _context;
        private IDbContextTransaction? _transaction;

        private IGenericRepository<Inspector>? _inspectors;
        private IGenericRepository<EntityToInspect>? _entitiesToInspect;
        private IGenericRepository<InspectionVisit>? _inspectionVisits;
        private IGenericRepository<Violation>? _violations;
        private IGenericRepository<ApplicationUser>? _applicationUser;

        public UnitOfWork(InspectionDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Inspector> Inspectors =>
            _inspectors ??= new GenericRepository<Inspector>(_context);

        public IGenericRepository<EntityToInspect> EntitiesToInspect =>
            _entitiesToInspect ??= new GenericRepository<EntityToInspect>(_context);

        public IGenericRepository<InspectionVisit> InspectionVisits =>
            _inspectionVisits ??= new GenericRepository<InspectionVisit>(_context);

        public IGenericRepository<Violation> Violations =>
            _violations ??= new GenericRepository<Violation>(_context);

        public IGenericRepository<ApplicationUser> applicationUser =>
            _applicationUser ??= new GenericRepository<ApplicationUser>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
