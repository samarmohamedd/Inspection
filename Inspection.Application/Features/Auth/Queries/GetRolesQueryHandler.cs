using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Inspection.Application.Dto;
using Inspection.Domain.Entities;

namespace Inspection.Application.Features.Auth.Queries
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;

        public GetRolesQueryHandler(RoleManager<ApplicationRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .OrderBy(r => r.Name)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }
    }
}
