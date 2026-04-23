using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IConfigurationNodeService
    {
        Task<List<ConfigurationNode>> GetAllAsync();
        Task<List<ConfigurationNode>> GetByProductIdAsync(int productId);
        Task<List<ConfigurationNode>> GetFlatByProductIdAsync(int productId);
        Task<ConfigurationNode> GetByIdAsync(int id);
        Task<(bool Success, string Message)> AddAsync(ConfigurationNode model);
        Task<(bool Success, string Message)> UpdateAsync(ConfigurationNode model);
        Task<(bool Success, string Message)> DeleteAsync(int id);
    }
}