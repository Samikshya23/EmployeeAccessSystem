using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IConfigurationService
    {
        Task<IEnumerable<Configuration>> GetAllAsync();
        Task<IEnumerable<Configuration>> GetByProductIdAsync(int productId);
        Task<Configuration> GetByIdAsync(int id);
        Task<string> AddAsync(Configuration model);
        Task<string> UpdateAsync(Configuration model);
        Task<string> DeleteAsync(int id);
        Task<string> ToggleAsync(int id);
    }
}