using EmployeeAccessSystem.Models;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Repositories
{
    public interface IAccountRepository
    {
        Task<int> CreateAsync(Account account);
        Task<Account?> GetByEmailAsync(string email);
    }
}
