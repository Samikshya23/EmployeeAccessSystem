using System.Threading.Tasks;
using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Services
{
    public interface IAccountService
    {
        Task<string?> RegisterAsync(RegisterModel model);
        Task<string?> LoginAsync(LoginModel model);
        Task<Account?> GetAccountByEmailAsync(string email);
    }
}
