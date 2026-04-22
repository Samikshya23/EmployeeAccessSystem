using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IFortigateConfigRepositories
    {
        Task<IEnumerable<FortigateConfig>> GetAllAsync();
        Task<FortigateConfig?> GetByIdAsync(int id);
        Task<FortigateConfig?> GetExistingAsync(int productId, int categoryId, int itemId, DateTime date);

        Task<int> AddAsync(FortigateConfig model);
        Task<int> UpdateAsync(FortigateConfig model);
        Task<int> DeleteAsync(int id, DateTime? deletedDate, string? deletedBy);
        Task<int> ToggleAsync(int id);
    }
}