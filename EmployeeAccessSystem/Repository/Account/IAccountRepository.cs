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
     
    }
}
