using Application.Wrappers;
using Domain;
using Mapster;
using MediatR;

namespace Application.Features.Companies.Queries;

public class GetCompanyByIdQuery : IRequest<IResponseWrapper>
{
    public int CompanyId { get; set; }
}

public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, IResponseWrapper>
{
    private readonly ICompanyService _companyService;

    public GetCompanyByIdQueryHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<IResponseWrapper> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var companyIdDb = await _companyService.GetByIdAsync(request.CompanyId);

        if (companyIdDb is not null)
        {
            return await ResponseWrapper<CompanyResponse>.SuccessAsync(data: companyIdDb.Adapt<CompanyResponse>());
        }
        return await ResponseWrapper<int>.FailAsync(message: "Organization does not exists.");
    }
}
