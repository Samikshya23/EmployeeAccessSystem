using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IFortigateConfigService
    {
        Task<IEnumerable<FortigateConfig>> GetAllAsync();
        Task<FortigateConfig?> GetByIdAsync(int id);
        Task<FortigateConfig?> GetExistingAsync(int productId, int categoryId, int itemId, DateTime date);

        Task<string> AddAsync(FortigateConfig model, string? currentUser);
        Task<string> UpdateAsync(FortigateConfig model, string? currentUser);
        Task<string> DeleteAsync(int id, string? currentUser);
        Task<string> ToggleAsync(int id);
    }
}