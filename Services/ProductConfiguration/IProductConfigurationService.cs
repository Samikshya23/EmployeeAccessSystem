using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IProductConfigurationService
    {
        Task<List<ProductConfiguration>> GetAllAsync();
        Task<(bool Success, string Message)> SaveStructureAsync(ProductConfigurationSaveRequest request);
    }
}