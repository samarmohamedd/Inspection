using MediatR;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.Entities.Commands
{
    public class CreateEntityCommand : IRequest<EntityToInspectDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public EntityCategory Category { get; set; }

        public CreateEntityCommand(CreateEntityToInspectDto createEntityDto)
        {
            Name = createEntityDto.Name;
            Address = createEntityDto.Address;
            Category = createEntityDto.Category;
        }
    }
}
