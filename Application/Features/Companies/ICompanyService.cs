using Domain;

namespace Application.Features.Companies;

public interface ICompanyService
{
    Task<int> CreateAsync(Company company);
    Task<int> UpdateAsync(Company company);
    Task<int> DeleteAsync(Company company);
    Task<Company?> GetByIdAsync(int companyId);
    Task<List<Company>> GetAllAsync();
    Task<Company?> GetByNameAsync(string name);
}
