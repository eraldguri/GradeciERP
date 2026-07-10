using Application.Features.Companies;
using Domain;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Companies;

public class OrganizationService(ApplicationDbContext context) : ICompanyService
{
    public async Task<int> CreateAsync(Organization organization)
    {
        await context.Organizations.AddAsync(organization);
        await context.SaveChangesAsync();
        return organization.Id;
    }

    public async Task<int> DeleteAsync(Organization organization)
    {
        context.Organizations.Remove(organization);
        await context.SaveChangesAsync();
        return organization.Id;
    }

    public async Task<List<Organization>> GetAllAsync()
    {
        return await context.Organizations.ToListAsync();
    }

    public async Task<Organization?> GetByIdAsync(int companyId)
    {
        return await context.Organizations
            .Where(company => company.Id == companyId)
            .FirstOrDefaultAsync();
    }

    public async Task<Organization?> GetByNameAsync(string name)
    {
        return await context.Organizations
            .Where(company => company.Name == name)
            .FirstOrDefaultAsync();
    }

    public async Task<int> UpdateAsync(Organization organization)
    {
        context.Organizations.Update(organization);
        await context.SaveChangesAsync();
        return organization.Id;
    }
}
