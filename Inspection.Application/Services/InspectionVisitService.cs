using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto; 
using Inspection.Domain.Entities;
using Inspection.Domain.Enum;
using Inspection.DataAccessLayer.Repository;

namespace Inspection.Application.Services
{
    public class InspectionVisitService : IInspectionVisitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public InspectionVisitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<InspectionVisitDto>> GetAllAsync(InspectionVisitFilterDto filter)
        {
            var query = _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(x => x.EntityToInspect)
                .Include(x => x.Inspector)
                .Include(x => x.Violations)
                .AsQueryable();

            // Apply filters
            if (filter.StartDate.HasValue)
                query = query.Where(x => x.ScheduledAt >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(x => x.ScheduledAt <= filter.EndDate.Value);

            if (filter.Status.HasValue)
                query = query.Where(x => x.Status == filter.Status.Value);

            if (filter.InspectorId.HasValue)
                query = query.Where(x => x.InspectorId == filter.InspectorId.Value);

            if (filter.Category.HasValue)
                query = query.Where(x => x.EntityToInspect.Category == filter.Category.Value);

            var totalCount = await query.CountAsync();

            var visits = await query
                .OrderByDescending(x => x.ScheduledAt)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            var visitDtos = _mapper.Map<List<InspectionVisitDto>>(visits);

            return new PagedResultDto<InspectionVisitDto>
            {
                Items = visitDtos,
                TotalCount = totalCount,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize
            };
        }

        public async Task<InspectionVisitDto?> GetByIdAsync(int id)
        {
            var visit = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(x => x.EntityToInspect)
                .Include(x => x.Inspector)
                .Include(x => x.Violations)
                .FirstOrDefaultAsync(x => x.Id == id);

            return visit != null ? _mapper.Map<InspectionVisitDto>(visit) : null;
        }

        public async Task<InspectionVisitDto> CreateAsync(CreateInspectionVisitDto createInspectionVisitDto)
        {
            // Validate that EntityToInspect and Inspector exist
            var entityExists = await _unitOfWork.EntitiesToInspect.GetAsQueryable().AnyAsync(x => x.Id == createInspectionVisitDto.EntityToInspectId && x.IsActive);
            if (!entityExists)
            {
                throw new InvalidOperationException("Entity to inspect not found or inactive");
            }

            var inspectorExists = await _unitOfWork.Inspectors.GetAsQueryable().AnyAsync(x => x.Id == createInspectionVisitDto.InspectorId && x.IsActive);
            if (!inspectorExists)
            {
                throw new InvalidOperationException("Inspector not found or inactive");
            }

            var visit = _mapper.Map<InspectionVisit>(createInspectionVisitDto);
            
            await _unitOfWork.InspectionVisits.AddAsync(visit);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(visit.Id) ?? throw new InvalidOperationException("Failed to retrieve created visit");
        }

        public async Task<InspectionVisitDto?> UpdateAsync(int id, UpdateInspectionVisitDto updateInspectionVisitDto)
        {
            var visit = await _unitOfWork.InspectionVisits.FindAsync(id);
            if (visit == null)
            {
                return null;
            }

            // Validate that EntityToInspect and Inspector exist
            var entityExists = await _unitOfWork.EntitiesToInspect.GetAsQueryable().AnyAsync(x => x.Id == updateInspectionVisitDto.EntityToInspectId && x.IsActive);
            if (!entityExists)
            {
                throw new InvalidOperationException("Entity to inspect not found or inactive");
            }

            var inspectorExists = await _unitOfWork.Inspectors.GetAsQueryable().AnyAsync(x => x.Id == updateInspectionVisitDto.InspectorId && x.IsActive);
            if (!inspectorExists)
            {
                throw new InvalidOperationException("Inspector not found or inactive");
            }

            _mapper.Map(updateInspectionVisitDto, visit);
            await _unitOfWork.InspectionVisits.UpdateAsync(visit);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<InspectionVisitDto?> CompleteAsync(int id, CompleteInspectionVisitDto completeInspectionVisitDto)
        {
            var visit = await _unitOfWork.InspectionVisits.FindAsync(id);
            if (visit == null)
            {
                return null;
            }

            if (visit.Status != InspectionStatus.InProgress)
            {
                throw new InvalidOperationException("Only in-progress visits can be completed");
            }

            visit.Status = InspectionStatus.Completed;
            visit.Score = completeInspectionVisitDto.Score;
            visit.Notes = completeInspectionVisitDto.Notes;
            visit.CompletedAt = DateTime.UtcNow;

            // Add violations
            foreach (var violationDto in completeInspectionVisitDto.Violations)
            {
                var violation = _mapper.Map<Violation>(violationDto);
                violation.InspectionVisitId = id;
                await _unitOfWork.Violations.AddAsync(violation);
            }

            await _unitOfWork.InspectionVisits.UpdateAsync(visit);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var visit = await _unitOfWork.InspectionVisits.FindAsync(id);
            if (visit == null)
            {
                return false;
            }

            // Only allow deletion of planned visits
            if (visit.Status != InspectionStatus.Planned)
            {
                throw new InvalidOperationException("Only planned visits can be deleted");
            }

            await _unitOfWork.InspectionVisits.DeleteAsync(visit);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.InspectionVisits.GetAsQueryable().AnyAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<InspectionVisitDto>> GetByInspectorAsync(int inspectorId)
        {
            var visits = await _unitOfWork.InspectionVisits.GetAsQueryable()
                .Include(x => x.EntityToInspect)
                .Include(x => x.Inspector)
                .Include(x => x.Violations)
                .Where(x => x.InspectorId == inspectorId)
                .OrderByDescending(x => x.ScheduledAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<InspectionVisitDto>>(visits);
        }

    }
}
