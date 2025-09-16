using MediatR;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.Entities.Commands
{
    public record UpdateEntityCommand : IRequest<EntityToInspectDto?>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public EntityCategory Category { get; set; }
        public bool IsActive { get; set; }

        public UpdateEntityCommand(int id, UpdateEntityToInspectDto updateEntityDto)
        {
            Id = id;
            Name = updateEntityDto.Name;
            Address = updateEntityDto.Address;
            Category = updateEntityDto.Category;
            IsActive = updateEntityDto.IsActive;
        }
    }
}
