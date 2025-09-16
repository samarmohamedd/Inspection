using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;

namespace Inspection.Application.Services
{
    public class InspectorService : IInspectorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public InspectorService(IUnitOfWork unitOfWork, IMapper mapper, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<IEnumerable<InspectorDto>> GetAllAsync()
        {
            var inspectors = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .Include(x => x.Role)
                .OrderBy(x => x.User.FullName)
                .ToListAsync();
            return _mapper.Map<IEnumerable<InspectorDto>>(inspectors);
        }

        public async Task<InspectorDto?> GetByIdAsync(int id)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
            return inspector != null ? _mapper.Map<InspectorDto>(inspector) : null;
        }

        public async Task<InspectorDto?> GetByEmailAsync(string email)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.User.Email == email);
            return inspector != null ? _mapper.Map<InspectorDto>(inspector) : null;
        }

        public async Task CreateAsync(CreateInspectorDto createInspectorDto)
        {
             var entity=_mapper.Map<Inspector>(createInspectorDto);

            await _unitOfWork.Inspectors.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<InspectorDto?> UpdateAsync(int id, UpdateInspectorDto updateInspectorDto)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (inspector == null)
            {
                return null;
            }

            // Update Inspector properties
            inspector.RoleId = updateInspectorDto.RoleId;
            inspector.IsActive = updateInspectorDto.IsActive;

            // Note: User properties (FullName, Email, Phone) should be updated through Identity UserManager
            // For now, we'll just update the Inspector-specific properties

            await _unitOfWork.Inspectors.UpdateAsync(inspector);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var inspector = await _unitOfWork.Inspectors.FindAsync(id);
            if (inspector == null)
            {
                return false;
            }

            // Check if inspector has any inspection visits
            var hasVisits = await _unitOfWork.InspectionVisits.GetAsQueryable().AnyAsync(x => x.InspectorId == id);
            if (hasVisits)
            {
                // Soft delete by deactivating
                inspector.IsActive = false;
                await _unitOfWork.Inspectors.UpdateAsync(inspector);
            }
            else
            {
                // Hard delete if no visits
                await _unitOfWork.Inspectors.DeleteAsync(inspector);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Inspectors.GetAsQueryable().AnyAsync(x => x.Id == id);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _unitOfWork.Inspectors.GetAsQueryable()
                    .Include(x => x.User)
                    .AnyAsync(x => x.User.Email == email && x.Id != excludeId.Value);
            }
            return await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .AnyAsync(x => x.User.Email == email);
        }
    }
}
