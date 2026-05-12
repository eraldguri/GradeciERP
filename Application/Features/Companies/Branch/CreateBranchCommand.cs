using Application.Pipelines;
using Application.Wrappers;
using Domain;
using Mapster;
using MediatR;

namespace Application.Features.Companies.Branch;

public class CreateBranchCommand : IRequest<IResponseWrapper>, IValidateMe
{
    public CreateBranchRequest? CreateBranchRequest { get; set; }
}

public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, IResponseWrapper>
{
    private readonly IBranchService _branchService;

    public CreateBranchCommandHandler(IBranchService branchService)
    {
        _branchService = branchService;
    }

    public async Task<IResponseWrapper> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        var newBranch = request.CreateBranchRequest.Adapt<CompanyBranch>();
        var branchId = await _branchService.CreateAsync(newBranch);
        return await ResponseWrapper<int>.SuccessAsync(data: branchId, message: "Branch created successfully.");
    }
}
