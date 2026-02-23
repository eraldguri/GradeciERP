using Application.Features.Companies.Commands;
using FluentValidation;

namespace Application.Features.Companies.Validations;

public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
{
    public CreateCompanyCommandValidator()
    {
        RuleFor(command => command.CreateCompany)
            .SetValidator(new CreateCompanyRequestValidator());
    }
}
