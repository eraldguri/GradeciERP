using Domain;
using FluentValidation;

namespace Application.Features.Companies.Validations;

internal class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest?>
{
    public UpdateCompanyRequestValidator(ICompanyService companyService)
    {
        RuleFor(request => request!.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await companyService.GetByIdAsync(id) is Organization companyInDb && companyInDb.Id == id)
            .WithMessage("Organization with the specified ID does not exist.");

        RuleFor(request => request!.Name)
           .NotEmpty().WithMessage("Organization name is required.")
           .MaximumLength(60).WithMessage("Organization name cannot exceed 60 characters.");

        RuleFor(request => request!.EstablishedDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Established date cannot be in the future.");
    }
}
