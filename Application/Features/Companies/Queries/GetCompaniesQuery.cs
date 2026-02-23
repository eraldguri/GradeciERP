using Application.Wrappers;
using Mapster;
using MediatR;

namespace Application.Features.Companies.Queries;

public class GetCompaniesQuery : IRequest<IResponseWrapper>
{
}

public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, IResponseWrapper>
{
    private readonly ICompanyService _companyService;
 
    public GetCompaniesQueryHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<IResponseWrapper> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
    {
        var companiesInDb = await _companyService.GetAllAsync();

        if (companiesInDb?.Count > 0)
        {
            return await ResponseWrapper<List<CompanyResponse>>
                .SuccessAsync(data: companiesInDb.Adapt<List<CompanyResponse>>());
        }

        return await ResponseWrapper<int>.FailAsync(message: "No companies were found.");
    }
}
