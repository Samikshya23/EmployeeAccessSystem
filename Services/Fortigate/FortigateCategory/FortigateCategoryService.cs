using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class FortigateCategoryService : IFortigateCategoryService
    {
        private readonly IFortigateCategoryRepository _repository;

        public FortigateCategoryService(IFortigateCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FortigateCategory>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<FortigateCategory>> GetActiveAsync()
        {
            return await _repository.GetActiveAsync();
        }

        public async Task<FortigateCategory> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(FortigateCategory model)
        {
            if (model == null)
            {
                return "Invalid fortigate category data.";
            }

            if (model.ProductId <= 0)
            {
                return "Please select product.";
            }

            if (string.IsNullOrWhiteSpace(model.CategoryName))
            {
                return "Category name is required.";
            }

            model.CategoryName = model.CategoryName.Trim();

            if (model.DisplayOrder < 0)
            {
                model.DisplayOrder = 0;
            }

            int result = await _repository.AddAsync(model);

            if (result > 0)
            {
                return "Fortigate category added successfully.";
            }

            return "Failed to add fortigate category.";
        }

        public async Task<string> UpdateAsync(FortigateCategory model)
        {
            if (model == null)
            {
                return "Invalid fortigate category data.";
            }

            if (model.FortigateCategoryId <= 0)
            {
                return "Invalid fortigate category id.";
            }

            if (model.ProductId <= 0)
            {
                return "Please select product.";
            }

            if (string.IsNullOrWhiteSpace(model.CategoryName))
            {
                return "Category name is required.";
            }

            model.CategoryName = model.CategoryName.Trim();

            if (model.DisplayOrder < 0)
            {
                model.DisplayOrder = 0;
            }

            int result = await _repository.UpdateAsync(model);

            if (result > 0)
            {
                return "Fortigate category updated successfully.";
            }

            return "Failed to update fortigate category.";
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid fortigate category id.";
            }

            int result = await _repository.DeleteAsync(id);

            if (result > 0)
            {
                return "Fortigate category deleted successfully.";
            }

            return "Failed to delete fortigate category.";
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid fortigate category id.";
            }

            int result = await _repository.ToggleAsync(id);

            if (result > 0)
            {
                return "Fortigate category status changed successfully.";
            }

            return "Failed to change fortigate category status.";
        }
    }
}