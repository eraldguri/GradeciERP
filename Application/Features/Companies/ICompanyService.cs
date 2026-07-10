using Domain;

namespace Application.Features.Companies;

public interface ICompanyService
{
    Task<int> CreateAsync(Organization organization);
    Task<int> UpdateAsync(Organization organization);
    Task<int> DeleteAsync(Organization organization);
    Task<Organization?> GetByIdAsync(int companyId);
    Task<List<Organization>> GetAllAsync();
    Task<Organization?> GetByNameAsync(string name);
}
