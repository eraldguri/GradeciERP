using Application.Features.Companies.Branch;
using Domain;
using Infrastructure.Context;

namespace Infrastructure.Companies;

public class BranchService : IBranchService
{
    private readonly ApplicationDbContext _context;

    public BranchService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(CompanyBranch branch)
    {
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();
        return branch.Id;
    }
}
