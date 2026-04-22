using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IConfigurationRepository
    {
        Task<IEnumerable<Configuration>> GetAllAsync();
        Task<IEnumerable<Configuration>> GetByProductIdAsync(int productId);
        Task<Configuration> GetByIdAsync(int id);
        Task<int> AddAsync(Configuration model);
        Task<int> UpdateAsync(Configuration model);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleAsync(int id);
    }
}