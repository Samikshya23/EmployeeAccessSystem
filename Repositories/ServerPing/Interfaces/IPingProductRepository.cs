using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IPingProductRepository
    {
        Task<IEnumerable<PingProduct>> GetAllAsync();
        Task<IEnumerable<PingProduct>> GetActiveAsync();
        Task<IEnumerable<PingProduct>> GetByProductIdAsync(int productId);
        Task<PingProduct> GetByIdAsync(int id);
        Task<PingProduct> CheckDuplicateAsync(string ipAddress, string serverHostName);
        Task<PingProduct> CheckDuplicateForUpdateAsync(int pingProductId, string ipAddress, string serverHostName);
        Task<int> AddAsync(PingProduct pingProduct);
        Task<int> UpdateAsync(PingProduct pingProduct);
        Task<int> DeleteAsync(int id);
        Task<int> ToggleAsync(int id);
    }
}