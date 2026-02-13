using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repo;

        public CategoryService(ICategoryRepository repo)
        {
            _repo = repo;

        }
        public async Task<List<Category>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.ToList();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(Category category)
        {
            if (category == null) return "Invalid data.";
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                return "Category name is required.";

            await _repo.AddAsync(category);
            return "";
        }

        public async Task<string> UpdateAsync(Category category)
        {
            if (category == null) return "Invalid data.";
            if (string.IsNullOrWhiteSpace(category.CategoryName))
                return "Category name is required.";

            await _repo.UpdateAsync(category);
            return "";
        }

        public async Task DeleteAsync(int id)
        {
            await _repo.DeleteAsync(id);
        }

        public async Task ToggleAsync(int id)
        {
            await _repo.ToggleAsync(id);
        }
    }
}
