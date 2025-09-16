using MediatR;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Application.Features.InspectionVisits.Commands
{
    public record UpdateVisitCommand : IRequest<InspectionVisitDto?>
    {
        public int Id { get; set; }
        public int EntityToInspectId { get; set; }
        public int InspectorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public InspectionStatus Status { get; set; }
        public int? Score { get; set; }
        public string? Notes { get; set; }

        public UpdateVisitCommand(int id, UpdateInspectionVisitDto updateVisitDto)
        {
            Id = id;
            EntityToInspectId = updateVisitDto.EntityToInspectId;
            InspectorId = updateVisitDto.InspectorId;
            ScheduledAt = updateVisitDto.ScheduledAt;
            Status = updateVisitDto.Status;
            Score = updateVisitDto.Score;
            Notes = updateVisitDto.Notes;
        }
    }
}
