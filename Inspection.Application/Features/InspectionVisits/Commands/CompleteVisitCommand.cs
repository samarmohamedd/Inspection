using MediatR;
using Inspection.Application.Dto;

namespace Inspection.Application.Features.InspectionVisits.Commands
{
    public class CompleteVisitCommand : IRequest<InspectionVisitDto?>
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string? Notes { get; set; }
        public List<CreateViolationDto> Violations { get; set; } = new List<CreateViolationDto>();

        public CompleteVisitCommand(int id, CompleteInspectionVisitDto completeVisitDto)
        {
            Id = id;
            Score = completeVisitDto.Score;
            Notes = completeVisitDto.Notes;
            Violations = completeVisitDto.Violations;
        }
    }
}
