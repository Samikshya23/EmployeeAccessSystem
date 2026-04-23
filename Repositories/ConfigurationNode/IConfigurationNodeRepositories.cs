using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IConfigurationNodeRepositories
    {
        Task<IEnumerable<ConfigurationNode>> GetAllAsync();
        Task<IEnumerable<ConfigurationNode>> GetByProductIdAsync(int productId);
        Task<ConfigurationNode> GetByIdAsync(int id);
        Task<int> AddAsync(ConfigurationNode model);
        Task<int> UpdateAsync(ConfigurationNode model);
        Task<int> DeleteAsync(int id);
    }
}