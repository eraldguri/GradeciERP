using Application.Wrappers;
using MediatR;

namespace Application.Features.Companies.Commands;

public class UpdateCompanyCommand : IRequest<IResponseWrapper>
{
    public UpdateCompanyRequest? UpdateCompany { get; set; }
}

public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, IResponseWrapper>
{
    private readonly ICompanyService _companyService;

    public UpdateCompanyCommandHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<IResponseWrapper> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyInDb = await _companyService.GetByIdAsync(request.UpdateCompany!.Id);

        if (companyInDb is not null)
        {
           companyInDb.Name = request.UpdateCompany.Name;
           companyInDb.Country = request.UpdateCompany.Country;
           companyInDb.TimeZone = request.UpdateCompany.TimeZone;
           companyInDb.Currency = request.UpdateCompany.Currency;
           companyInDb.EstablishedDate = request.UpdateCompany.EstablishedDate;

            var updatedCompanyId = await _companyService.UpdateAsync(companyInDb);

            return await ResponseWrapper<int>.SuccessAsync(data: updatedCompanyId, "Company updated successfully");
        }
        return await ResponseWrapper<int>.FailAsync("Company does not exists.");
    }
}
