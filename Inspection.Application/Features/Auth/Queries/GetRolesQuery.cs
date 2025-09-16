using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.Auth.Queries
{
    public record GetRolesQuery : IRequest<IEnumerable<RoleDto>>;
}
