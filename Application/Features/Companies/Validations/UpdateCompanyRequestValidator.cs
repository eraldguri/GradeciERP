using Domain;
using FluentValidation;

namespace Application.Features.Companies.Validations;

internal class UpdateCompanyRequestValidator : AbstractValidator<UpdateCompanyRequest?>
{
    public UpdateCompanyRequestValidator(ICompanyService companyService)
    {
        RuleFor(request => request!.Id)
            .NotEmpty()
            .MustAsync(async (id, ct) => await companyService.GetByIdAsync(id) is Company companyInDb && companyInDb.Id == id)
            .WithMessage("Company with the specified ID does not exist.");

        RuleFor(request => request!.Name)
           .NotEmpty().WithMessage("Company name is required.")
           .MaximumLength(60).WithMessage("Company name cannot exceed 60 characters.");

        RuleFor(request => request!.EstablishedDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Established date cannot be in the future.");
    }
}
