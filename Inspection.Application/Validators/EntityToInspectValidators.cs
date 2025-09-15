using FluentValidation;
using Inspection.Application.Dto;
using Inspection.Domain.Enum;

namespace Inspection.Application.Validators
{
    public class CreateEntityToInspectDtoValidator : AbstractValidator<CreateEntityToInspectDto>
    {
        public CreateEntityToInspectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

            RuleFor(x => x.Category)
                .IsInEnum().WithMessage("Category must be a valid category");
        }
    }

    public class UpdateEntityToInspectDtoValidator : AbstractValidator<UpdateEntityToInspectDto>
    {
        public UpdateEntityToInspectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters");

            RuleFor(x => x.Category)
                .IsInEnum().WithMessage("Category must be a valid category");
        }
    }
}
