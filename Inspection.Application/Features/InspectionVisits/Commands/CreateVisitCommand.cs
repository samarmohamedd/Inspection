using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.InspectionVisits.Commands
{
    public record CreateVisitCommand : IRequest<InspectionVisitDto>
    {
        public int EntityToInspectId { get; set; }
        public int InspectorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string? Notes { get; set; }

        public CreateVisitCommand(CreateInspectionVisitDto createVisitDto)
        {
            EntityToInspectId = createVisitDto.EntityToInspectId;
            InspectorId = createVisitDto.InspectorId;
            ScheduledAt = createVisitDto.ScheduledAt;
            Notes = createVisitDto.Notes;
        }
    }
}
