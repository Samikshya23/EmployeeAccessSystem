using EmployeeAccessSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public interface IProductSetupService
    {
        Task<IEnumerable<ProductSetup>> GetAllAsync();
        Task<ProductSetup> GetByIdAsync(int id);
        Task<string> AddAsync(ProductSetup productSetup);
        Task<string> UpdateAsync(ProductSetup productSetup);
        Task<string> DeleteAsync(int id);
        Task<string> ToggleAsync(int id);
    }
}