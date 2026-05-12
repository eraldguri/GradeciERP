using Domain;

namespace Application.Features.Companies.Branch;

public interface IBranchService
{
    Task<int> CreateAsync(CompanyBranch branch);
}
