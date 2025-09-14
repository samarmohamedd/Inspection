using Microsoft.EntityFrameworkCore;
using Inspection.Application.Abstraction;
using Inspection.DataAccessLayer.Context;
using Inspection.Domain.Entities;
using Inspection.Domain.Enum;

namespace Inspection.DataAccessLayer.Repository;

/// <summary>
/// Inspection visit repository implementation with specific operations
/// </summary>
public class InspectionVisitRepository : Repository<InspectionVisit>, IInspectionVisitRepository
{
    /// <summary>
    /// Initializes a new instance of the InspectionVisitRepository class
    /// </summary>
    /// <param name="context">Database context</param>
    public InspectionVisitRepository(ApplicationDbContext context) : base(context)
    {
    }

    /// <inheritdoc />
    public async Task<List<InspectionVisit>> GetByBusinessIdAsync(int businessId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(iv => iv.Inspector)
            .Where(iv => iv.BusinessId == businessId)
            .OrderByDescending(iv => iv.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<InspectionVisit>> GetByInspectorIdAsync(int inspectorId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(iv => iv.Business)
            .Where(iv => iv.InspectorId == inspectorId)
            .OrderByDescending(iv => iv.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<InspectionVisit>> GetByStatusAsync(InspectionStatus status, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(iv => iv.Business)
            .Include(iv => iv.Inspector)
            .Where(iv => iv.Status == status)
            .OrderBy(iv => iv.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<InspectionVisit>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(iv => iv.Business)
            .Include(iv => iv.Inspector)
            .Where(iv => iv.InspectionDate.Date >= startDate.Date && iv.InspectionDate.Date <= endDate.Date)
            .OrderBy(iv => iv.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<InspectionVisit?> GetWithDetailsAsync(int visitId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(iv => iv.Business)
            .Include(iv => iv.Inspector)
            .Include(iv => iv.InspectionResults)
                .ThenInclude(ir => ir.InspectionItem)
            .FirstOrDefaultAsync(iv => iv.Id == visitId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<InspectionVisit>> GetUpcomingInspectionsAsync(int inspectorId, int days = 7, CancellationToken cancellationToken = default)
    {
        var startDate = DateTime.UtcNow.Date;
        var endDate = startDate.AddDays(days);

        return await _dbSet
            .Include(iv => iv.Business)
            .Where(iv => iv.InspectorId == inspectorId &&
                        iv.InspectionDate.Date >= startDate &&
                        iv.InspectionDate.Date <= endDate &&
                        (iv.Status == InspectionStatus.Scheduled || iv.Status == InspectionStatus.InProgress))
            .OrderBy(iv => iv.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task<List<InspectionVisit>> GetOverdueInspectionsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;

        return await _dbSet
            .Include(iv => iv.Business)
            .Include(iv => iv.Inspector)
            .Where(iv => iv.FollowUpDate.HasValue &&
                        iv.FollowUpDate.Value.Date < today &&
                        iv.Status != InspectionStatus.Completed &&
                        iv.Status != InspectionStatus.Cancelled)
            .OrderBy(iv => iv.FollowUpDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<List<InspectionVisit>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(iv => iv.Business)
            .Include(iv => iv.Inspector)
            .OrderByDescending(iv => iv.InspectionDate)
            .ToListAsync(cancellationToken);
    }

    /// <inheritdoc />
    public override async Task<(List<InspectionVisit> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        var totalCount = await _dbSet.CountAsync(cancellationToken);
        var items = await _dbSet
            .Include(iv => iv.Business)
            .Include(iv => iv.Inspector)
            .OrderByDescending(iv => iv.InspectionDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
