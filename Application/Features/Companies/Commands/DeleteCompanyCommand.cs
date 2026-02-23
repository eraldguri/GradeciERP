using Application.Wrappers;
using MediatR;

namespace Application.Features.Companies.Commands;

public class DeleteCompanyCommand : IRequest<IResponseWrapper>
{
    public int CompanyId { get; set; }
}

public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, IResponseWrapper>
{
    private readonly ICompanyService _companyService;

    public DeleteCompanyCommandHandler(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    public async Task<IResponseWrapper> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyInDb = await _companyService.GetByIdAsync(request.CompanyId);
        if (companyInDb is not null)
        {
            var deletedCompanyId = await _companyService.DeleteAsync(companyInDb);

            return await ResponseWrapper<int>.SuccessAsync(data: deletedCompanyId, "Company deleted successfully");
        }
        return await ResponseWrapper<int>.FailAsync("Company does not exists.");
    }
}
