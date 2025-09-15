using FluentValidation;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Application.Validators
{
    public class CreateInspectionVisitDtoValidator : AbstractValidator<CreateInspectionVisitDto>
    {
        public CreateInspectionVisitDtoValidator()
        {
            RuleFor(x => x.EntityToInspectId)
                .GreaterThan(0).WithMessage("Entity to inspect ID must be greater than 0");

            RuleFor(x => x.InspectorId)
                .GreaterThan(0).WithMessage("Inspector ID must be greater than 0");

            RuleFor(x => x.ScheduledAt)
                .NotEmpty().WithMessage("Scheduled date is required")
                .GreaterThan(DateTime.UtcNow).WithMessage("Scheduled date must be in the future");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");
        }
    }

    public class UpdateInspectionVisitDtoValidator : AbstractValidator<UpdateInspectionVisitDto>
    {
        public UpdateInspectionVisitDtoValidator()
        {
            RuleFor(x => x.EntityToInspectId)
                .GreaterThan(0).WithMessage("Entity to inspect ID must be greater than 0");

            RuleFor(x => x.InspectorId)
                .GreaterThan(0).WithMessage("Inspector ID must be greater than 0");

            RuleFor(x => x.ScheduledAt)
                .NotEmpty().WithMessage("Scheduled date is required");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status");

            RuleFor(x => x.Score)
                .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100")
                .When(x => x.Score.HasValue);

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");
        }
    }

    public class CompleteInspectionVisitDtoValidator : AbstractValidator<CompleteInspectionVisitDto>
    {
        public CompleteInspectionVisitDtoValidator()
        {
            RuleFor(x => x.Score)
                .InclusiveBetween(0, 100).WithMessage("Score must be between 0 and 100");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters");

            RuleForEach(x => x.Violations)
                .SetValidator(new CreateViolationDtoValidator());
        }
    }
}
