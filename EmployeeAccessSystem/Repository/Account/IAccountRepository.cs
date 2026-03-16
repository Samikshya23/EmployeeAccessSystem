using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface IAccountRepository
    {
        Task<int> CreateAsync(Account account);
        Task<Account?> GetByEmailAsync(string email);
        Task<Account?> GetByIdAsync(int id);
        Task<int> UpdateAsync(Account account);
        Task<int> DeleteAsync(int accountId);
        Task<int> AssignRoleAsync(int accountId, int roleId);
        Task<int> UpdateRoleAsync(int accountId, int roleId);
    }
}