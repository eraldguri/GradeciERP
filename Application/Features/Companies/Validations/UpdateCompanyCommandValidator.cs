using Application.Features.Companies.Commands;
using FluentValidation;

namespace Application.Features.Companies.Validations;

public class UpdateCompanyCommandValidator : AbstractValidator<UpdateCompanyCommand>
{
    public UpdateCompanyCommandValidator(ICompanyService companyService)
    {
        RuleFor(command => command.UpdateCompany)
            .SetValidator(new UpdateCompanyRequestValidator(companyService));
    }
}
