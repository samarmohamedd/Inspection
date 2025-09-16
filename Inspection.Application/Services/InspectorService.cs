using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Inspection.Application.Abstractions;
using Inspection.Application.Dto;
using Inspection.DataAccessLayer.Repository;
using Inspection.Domain.Entities;
using Inspection.Domain.Constants;

namespace Inspection.Application.Services
{
    public class InspectorService : IInspectorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public InspectorService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<InspectorDto>> GetAllAsync()
        {
            var inspectors = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .OrderBy(x => x.User.FullName)
                .ToListAsync();

            var inspectorDtos = _mapper.Map<IEnumerable<InspectorDto>>(inspectors);

            var result = new List<InspectorDto>();
            foreach (var dto in inspectorDtos)
            {
                var inspector = inspectors.First(i => i.Id == dto.Id);
                var populatedDto = _mapper.Map<InspectorDto>(dto);
                result.Add(populatedDto);
            }

            return result;
        }

        public async Task<InspectorDto?> GetByIdAsync(int id)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (inspector == null)
                return null;

            var inspectorDto = _mapper.Map<InspectorDto>(inspector);
            return inspectorDto;
        }

        public async Task<InspectorDto?> GetByEmailAsync(string email)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Email == email);

            if (inspector == null)
                return null;

            var inspectorDto = _mapper.Map<InspectorDto>(inspector);
            return inspectorDto;
        }

        public async Task CreateAsync(CreateInspectorDto createInspectorDto)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(createInspectorDto.Email);
                if (existingUser != null)
                {
                    throw new InvalidOperationException("An account with this email address already exists.");
                }

                var role = await _roleManager.FindByNameAsync(RoleConstants.Names.Inspector);
                if (role == null)
                {
                    throw new InvalidOperationException("Inspector role not found in the system. Please contact administrator.");
                }

                var user = new ApplicationUser
                {
                    UserName = createInspectorDto.Email,
                    Email = createInspectorDto.Email,
                    FullName = createInspectorDto.FullName,
                    Phone = createInspectorDto.Phone,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var userResult = await _userManager.CreateAsync(user, createInspectorDto.Password);
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create user: {string.Join(", ", userResult.Errors.Select(e => e.Description))}");
                }

                await _userManager.AddToRoleAsync(user, role.Name!);

                var inspector = new Inspector
                {
                    UserId = user.Id,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Inspectors.AddAsync(inspector);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to create inspector. Please check your connection and try again.", ex);
            }
        }

        public async Task<InspectorDto?> UpdateAsync(int id, UpdateInspectorDto updateInspectorDto)
        {
            var inspector = await _unitOfWork.Inspectors.GetAsQueryable()
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (inspector == null)
            {
                return null;
            }

            inspector.IsActive = updateInspectorDto.IsActive;
            inspector.UpdatedAt = DateTime.UtcNow;

            var user = inspector.User;
            user.FullName = updateInspectorDto.FullName;
            user.Phone = updateInspectorDto.Phone;
            user.UpdatedAt = DateTime.UtcNow;

            if (user.Email != updateInspectorDto.Email)
            {
                user.Email = updateInspectorDto.Email;
                user.UserName = updateInspectorDto.Email;
            }
            var userUpdateResult = await _userManager.UpdateAsync(user);
            if (!userUpdateResult.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update user: {string.Join(", ", userUpdateResult.Errors.Select(e => e.Description))}");
            }

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

            var hasVisits = await _unitOfWork.InspectionVisits.GetAsQueryable().AnyAsync(x => x.InspectorId == id);
            if (hasVisits)
            {
                inspector.IsActive = false;
                await _unitOfWork.Inspectors.UpdateAsync(inspector);
            }
            else
            {
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
