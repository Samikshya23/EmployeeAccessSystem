using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepositories _employeeRepo;
        private readonly IAccountRepositories _accountRepo;

        public EmployeeService(IEmployeeRepositories employeeRepo, IAccountRepositories accountRepo)
        {
            _employeeRepo = employeeRepo;
            _accountRepo = accountRepo;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _employeeRepo.GetAllAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            return await _employeeRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Employee>> GetSupervisorsAsync()
        {
            return await _employeeRepo.GetSupervisorsAsync();
        }

        public async Task<string> UpdateAsync(Employee model)
        {
            if (model == null)
            {
                return "Invalid data";
            }

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                return "Full Name is required";
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return "Email is required";
            }

            if (model.DepartmentId <= 0)
            {
                return "Department is required";
            }

            if (string.IsNullOrWhiteSpace(model.Role))
            {
                return "Role is required";
            }

            model.FullName = model.FullName.Trim();
            model.Email = model.Email.Trim().ToLower();
            model.Role = model.Role.Trim();

            var existing = await _employeeRepo.GetByEmailAsync(model.Email);
            if (existing != null && existing.EmployeeId != model.EmployeeId)
            {
                return "Email already used by another employee";
            }

            if (model.SupervisorEmployeeId == model.EmployeeId)
            {
                return "Employee cannot be their own supervisor";
            }

            int result = await _employeeRepo.UpdateAsync(model);
            if (result <= 0)
            {
                return "Employee update failed";
            }

            int roleId = GetRoleIdByName(model.Role);
            if (roleId <= 0)
            {
                return "Invalid role selected";
            }

            int roleResult = await _accountRepo.UpdateRoleAsync(model.AccountId, roleId);
            if (roleResult <= 0)
            {
                return "Role update failed";
            }

            return "";
        }

        public async Task<string> ToggleAsync(int id)
        {
            var emp = await _employeeRepo.GetByIdAsync(id);
            if (emp == null)
            {
                return "Employee not found";
            }

            await _employeeRepo.ToggleAsync(id);
            return "";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var emp = await _employeeRepo.GetByIdAsync(id);
            if (emp == null)
            {
                return "Employee not found";
            }

            await _accountRepo.DeleteAsync(emp.AccountId);
            return "";
        }

        private int GetRoleIdByName(string roleName)
        {
            if (roleName == "Admin")
            {
                return 1;
            }

            if (roleName == "Employee")
            {
                return 2;
            }

            if (roleName == "Supervisor")
            {
                return 3;
            }

            return 0;
        }
    }
}