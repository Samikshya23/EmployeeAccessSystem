using EmployeeAccessSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public interface IProductSetupService
    {
        Task<List<ProductSetup>> GetAllAsync();
        Task<ProductSetup?> GetByIdAsync(int id);
        Task AddAsync(ProductSetup productSetup);
        Task UpdateAsync(ProductSetup productSetup);
        Task DeleteAsync(int id);
        Task ToggleAsync(int id);
    }
}