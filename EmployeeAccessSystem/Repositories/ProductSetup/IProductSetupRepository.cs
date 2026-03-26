using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IProductSetupRepositories
    {
        Task<IEnumerable<ProductSetup>> GetAllAsync();
        Task<ProductSetup> GetByIdAsync(int id);
        Task<int> AddAsync(ProductSetup productSetup);
        Task<int> UpdateAsync(ProductSetup productSetup);
        Task<int> DeleteAsync(int id);
        Task ToggleAsync(int id);
        Task<IEnumerable<ProductSetup>> GetActiveAsync();
    }
}