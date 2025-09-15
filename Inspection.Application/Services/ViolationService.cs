using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;

namespace Inspection.Application.Services
{
    public class ViolationService : IViolationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ViolationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ViolationDto>> GetByInspectionVisitAsync(int inspectionVisitId)
        {
            var violations = await _unitOfWork.Violations.GetAsQueryable()
                .Where(x => x.InspectionVisitId == inspectionVisitId)
                .OrderBy(x => x.Description)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ViolationDto>>(violations);
        }

        public async Task<ViolationDto?> GetByIdAsync(int id)
        {
            var violation = await _unitOfWork.Violations.FindAsync(id);
            return violation != null ? _mapper.Map<ViolationDto>(violation) : null;
        }

        public async Task<ViolationDto> CreateAsync(int inspectionVisitId, CreateViolationDto createViolationDto)
        {
            // Validate that inspection visit exists
            var visitExists = await _unitOfWork.InspectionVisits.GetAsQueryable().AnyAsync(x => x.Id == inspectionVisitId);
            if (!visitExists)
            {
                throw new InvalidOperationException("Inspection visit not found");
            }

            var violation = _mapper.Map<Violation>(createViolationDto);
            violation.InspectionVisitId = inspectionVisitId;

            await _unitOfWork.Violations.AddAsync(violation);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(violation.Id) ?? throw new InvalidOperationException("Failed to retrieve created violation");
        }

        public async Task<ViolationDto?> UpdateAsync(int id, UpdateViolationDto updateViolationDto)
        {
            var violation = await _unitOfWork.Violations.FindAsync(id);
            if (violation == null)
            {
                return null;
            }

            _mapper.Map(updateViolationDto, violation);
            await _unitOfWork.Violations.UpdateAsync(violation);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var violation = await _unitOfWork.Violations.FindAsync(id);
            if (violation == null)
            {
                return false;
            }

            await _unitOfWork.Violations.DeleteAsync(violation);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.Violations.GetAsQueryable().AnyAsync(x => x.Id == id);
        }
    }
}
