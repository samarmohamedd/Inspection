using Microsoft.EntityFrameworkCore;
using Inspection.Application.Abstraction;
using Inspection.DataAccessLayer.Context;
using Inspection.Domain.Entities;
using Inspection.Domain.Enum;

namespace Inspection.DataAccessLayer.Repository;

/// <summary>
/// Business repository implementation with specific business operations
/// </summary>
public class BusinessRepository : Repository<Business>, IBusinessRepository
{
    /// <summary>
    /// Initializes a new instance of the BusinessRepository class
    /// </summary>
    /// <param name="context">Database context</param>
    public BusinessRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<List<Business>> GetByTypeAsync(BusinessType businessType, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(b => b.BusinessType == businessType)
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Business>> GetByCityAsync(string city, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(b => b.City.ToLower() == city.ToLower())
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Business>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(b => b.Name.ToLower().Contains(searchTerm.ToLower()))
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<Business?> GetWithInspectionHistoryAsync(int businessId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(b => b.InspectionVisits)
                .ThenInclude(iv => iv.Inspector)
            .Include(b => b.InspectionVisits)
                .ThenInclude(iv => iv.InspectionResults)
                    .ThenInclude(ir => ir.InspectionItem)
            .FirstOrDefaultAsync(b => b.Id == businessId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<Business>> GetBusinessesNeedingFollowUpAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        
        return await _dbSet
            .Include(b => b.InspectionVisits)
            .Where(b => b.InspectionVisits.Any(iv => 
                iv.Status == InspectionStatus.RequiresFollowUp ||
                (iv.FollowUpDate.HasValue && iv.FollowUpDate.Value.Date <= today && 
                 iv.Status != InspectionStatus.Completed)))
            .OrderBy(b => b.InspectionVisits
                .Where(iv => iv.FollowUpDate.HasValue)
                .Min(iv => iv.FollowUpDate))
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<List<Business>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .OrderBy(b => b.Name)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<(List<Business> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await _dbSet.CountAsync(cancellationToken);
        var items = await _dbSet
            .OrderBy(b => b.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
