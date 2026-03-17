using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepositories _repo;

        public CategoryService(ICategoryRepositories repo)
        {
            _repo = repo;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.ToList();
        }

        public Task<Category?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public Task AddAsync(Category category)
        {
            return _repo.AddAsync(category);
        }

        public Task UpdateAsync(Category category)
        {
            return _repo.UpdateAsync(category);
        }

        public Task DeleteAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }

        public Task ToggleAsync(int id)
        {
            return _repo.ToggleAsync(id);
        }
    }
}