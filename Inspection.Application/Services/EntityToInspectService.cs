using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;

namespace Inspection.Application.Services
{
    public class EntityToInspectService : IEntityToInspectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntityToInspectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EntityToInspectDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.EntitiesToInspect.GetAsQueryable()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .ToListAsync();
            return _mapper.Map<IEnumerable<EntityToInspectDto>>(entities);
        }

        public async Task<EntityToInspectDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.EntitiesToInspect.FindAsync(id);
            return entity != null ? _mapper.Map<EntityToInspectDto>(entity) : null;
        }

        public async Task<EntityToInspectDto> CreateAsync(CreateEntityToInspectDto createEntityToInspectDto)
        {
            var entity = _mapper.Map<EntityToInspect>(createEntityToInspectDto);

            await _unitOfWork.EntitiesToInspect.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(entity.Id) ?? throw new InvalidOperationException("Failed to retrieve created entity");
        }

        public async Task<EntityToInspectDto?> UpdateAsync(int id, UpdateEntityToInspectDto updateEntityToInspectDto)
        {
            var entity = await _unitOfWork.EntitiesToInspect.FindAsync(id);
            if (entity == null)
            {
                return null;
            }

            _mapper.Map(updateEntityToInspectDto, entity);
            await _unitOfWork.EntitiesToInspect.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _unitOfWork.EntitiesToInspect.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            // Check if entity has any inspection visits
            var hasVisits = await _unitOfWork.InspectionVisits.GetAsQueryable().AnyAsync(x => x.EntityToInspectId == id);
            if (hasVisits)
            {
                // Soft delete by deactivating
                entity.IsActive = false;
                await _unitOfWork.EntitiesToInspect.UpdateAsync(entity);
            }
            else
            {
                // Hard delete if no visits
                await _unitOfWork.EntitiesToInspect.DeleteAsync(entity);
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _unitOfWork.EntitiesToInspect.GetAsQueryable().AnyAsync(x => x.Id == id && x.IsActive);
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            var categories = await _unitOfWork.EntitiesToInspect.GetAsQueryable()
                .Where(x => x.IsActive)
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
            return categories;
        }
    }
}
