using Application.Wrappers;
using Mapster;
using MediatR;

namespace Application.Features.Companies.Queries;

public class GetCompanyByNameQuery : IRequest<IResponseWrapper>
{
    public string? Name { get; set; }
}

public class GetCompanyByNameQueryHandler : IRequestHandler<GetCompanyByNameQuery, IResponseWrapper>
{
    private readonly ICompanyService _companyService;
 
    public GetCompanyByNameQueryHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    
    public async Task<IResponseWrapper> Handle(GetCompanyByNameQuery request, CancellationToken cancellationToken)
    {
        var companyIdDb = await _companyService.GetByNameAsync(request.Name!);

        if (companyIdDb is not null)
        {
            return await ResponseWrapper<CompanyResponse>.SuccessAsync(data: companyIdDb.Adapt<CompanyResponse>());
        }
        return await ResponseWrapper<CompanyResponse>.FailAsync(message: "Company does not exists.");
    }
}
