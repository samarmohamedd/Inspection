using FluentValidation;
using Inspection.Application.Dto;

namespace Inspection.Application.Validators
{
    public class CreateViolationDtoValidator : AbstractValidator<CreateViolationDto>
    {
        public CreateViolationDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(50).WithMessage("Code cannot exceed 50 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.Severity)
                .IsInEnum().WithMessage("Invalid severity");
        }
    }

    public class UpdateViolationDtoValidator : AbstractValidator<UpdateViolationDto>
    {
        public UpdateViolationDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required")
                .MaximumLength(50).WithMessage("Code cannot exceed 50 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

            RuleFor(x => x.Severity)
                .IsInEnum().WithMessage("Invalid severity");
        }
    }
}
