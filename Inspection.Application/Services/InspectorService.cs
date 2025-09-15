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
                .OrderBy(x => x.FullName)
                .ToListAsync();
            return _mapper.Map<IEnumerable<InspectorDto>>(inspectors);
        }

        public async Task<InspectorDto?> GetByIdAsync(int id)
        {
            var inspector = await _unitOfWork.Inspectors.FindAsync(id);
            return inspector != null ? _mapper.Map<InspectorDto>(inspector) : null;
        }

        public async Task<InspectorDto?> GetByEmailAsync(string email)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .FirstOrDefaultAsync(x => x.Email == email);
            return inspector != null ? _mapper.Map<InspectorDto>(inspector) : null;
        }

        public async Task<InspectorDto> CreateAsync(CreateInspectorDto createInspectorDto)
        {
            if (await EmailExistsAsync(createInspectorDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var inspector = _mapper.Map<Inspector>(createInspectorDto);
            inspector.PasswordHash = _authService.HashPassword(createInspectorDto.Password);

            await _unitOfWork.Inspectors.AddAsync(inspector);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(inspector.Id) ?? throw new InvalidOperationException("Failed to retrieve created inspector");
        }

        public async Task<InspectorDto?> UpdateAsync(int id, UpdateInspectorDto updateInspectorDto)
        {
            var inspector = await _unitOfWork.Inspectors.FindAsync(id);
            if (inspector == null)
            {
                return null;
            }

            if (await EmailExistsAsync(updateInspectorDto.Email, id))
            {
                throw new InvalidOperationException("Email already exists");
            }

            _mapper.Map(updateInspectorDto, inspector);
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
                return await _unitOfWork.Inspectors.GetAsQueryable().AnyAsync(x => x.Email == email && x.Id != excludeId.Value);
            }
            return await _unitOfWork.Inspectors.GetAsQueryable().AnyAsync(x => x.Email == email);
        }
    }
}
