using Microsoft.Extensions.Configuration;
using EmployeeAccessSystem.Helpers;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IConfiguration _config;

        public AccountService(IAccountRepository accountRepo, IEmployeeRepository employeeRepo, IConfiguration config)
        {
            _accountRepo = accountRepo;
            _employeeRepo = employeeRepo;
            _config = config;
        }
        public async Task<string> RegisterAsync(RegisterModel model)
        { 
            if (string.IsNullOrWhiteSpace(model.FullName) ||
                string.IsNullOrWhiteSpace(model.Email) ||
                string.IsNullOrWhiteSpace(model.Password))
            {
                return "All fields are required";
            }

            if (model.DepartmentId <= 0)
            {
                return "Please select a department";
            }

            if (model.Password != model.ConfirmPassword)
            {
                return "Passwords do not match";
            }

          
            model.FullName = model.FullName.Trim();
            model.Email = model.Email.Trim().ToLower();

        
            Account existing = await _accountRepo.GetByEmailAsync(model.Email);
            if (existing != null)
            {
                return "Email already registered";
            }

     
            string key = _config["Security:PasswordKey"];
            Helper.CreatePasswordHash(model.Password, key, out byte[] hash, out byte[] salt);

          
            Account account = new Account
            {
                FullName = model.FullName,
                Email = model.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };

            int accountId = await _accountRepo.CreateAsync(account);

           
            Employee employee = new Employee
            {
                FullName = model.FullName,
                Email = model.Email,
                DepartmentId = model.DepartmentId,
                AccountId = accountId
            };

            int empResult = await _employeeRepo.AddAsync(employee);

        
            if (empResult <= 0)
            {
                await _accountRepo.DeleteAsync(accountId);
                return "Registration failed. Please try again.";
            }

            return ""; 
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return "Email should not be empty";
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return "Password should not be empty";
            }

            model.Email = model.Email.Trim().ToLower();

            Account account = await _accountRepo.GetByEmailAsync(model.Email);

            if (account == null)
            {
                return "Account not registered";
            }

            string key = _config["Security:PasswordKey"];

            bool ok = Helper.VerifyPassword(
                model.Password,
                key,
                account.PasswordHash,
                account.PasswordSalt
            );

            if (!ok)
            {
                return "Invalid password";
            }

            return ""; 
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            email = (email ?? "").Trim().ToLower();
            Account account = await _accountRepo.GetByEmailAsync(email);
            return account; 
        }
    }
}
