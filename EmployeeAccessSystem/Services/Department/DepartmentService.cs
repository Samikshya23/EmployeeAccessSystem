using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepositories _repository;

        public DepartmentService(IDepartmentRepositories repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Department>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Department> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task AddAsync(Department department)
        {
            return _repository.AddAsync(department);
        }

        public Task UpdateAsync(Department department)
        {
            return _repository.UpdateAsync(department);
        }

        public Task DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}