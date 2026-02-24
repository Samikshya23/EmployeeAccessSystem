
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IAccountRepository _accountRepo;

        public EmployeeService(IEmployeeRepository employeeRepo, IAccountRepository accountRepo)
        {
            _employeeRepo = employeeRepo;
            _accountRepo = accountRepo;
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _employeeRepo.GetByIdAsync(id);
        }

        public async Task<string> UpdateAsync(Employee model)
        {
            if (model == null)
            {
                return "Invalid data";
            }

            if (string.IsNullOrWhiteSpace(model.FullName) ||
                string.IsNullOrWhiteSpace(model.Email))
            {
                return "Full Name and Email are required";
            }

            if (model.DepartmentId <= 0)
            {
                return "Please select a department";
            }

            model.FullName = model.FullName.Trim();
            model.Email = model.Email.Trim().ToLower();

            // Duplicate email check (uses your repo method)
            Employee existing = await _employeeRepo.GetByEmailAsync(model.Email);
            if (existing != null && existing.EmployeeId != model.EmployeeId)
            {
                return "Email already used by another employee";
            }

            int result = await _employeeRepo.UpdateAsync(model);
            if (result <= 0)
            {
                return "Update failed. Please try again.";
            }

            return "";
        }

        public async Task<string> ToggleAsync(int id)
        {
            Employee emp = await _employeeRepo.GetByIdAsync(id);
            if (emp == null)
            {
                return "Employee not found";
            }

            await _employeeRepo.ToggleAsync(id);
            return "";
        }

        public async Task<string> DeleteAsync(int id)
        {
            Employee emp = await _employeeRepo.GetByIdAsync(id);
            if (emp == null)
            {
                return "Employee not found";
            }

            // Same as your controller logic: delete linked account
            await _accountRepo.DeleteAsync(emp.AccountId);
            return "";
        }
    }
}