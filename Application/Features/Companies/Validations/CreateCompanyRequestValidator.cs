using FluentValidation;

namespace Application.Features.Companies.Validations;

internal class CreateCompanyRequestValidator : AbstractValidator<CreateCompanyRequest?>
{
    public CreateCompanyRequestValidator()
    {
        RuleFor(request => request!.Name)
            .NotEmpty().WithMessage("Company name is required.")
            .MaximumLength(60).WithMessage("Company name cannot exceed 60 characters.");

        RuleFor(request => request!.EstablishedDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Established date cannot be in the future.");
    }
}
