using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class FortigateItemService : IFortigateItemService
    {
        private readonly IFortigateItemRepositories _repository;

        public FortigateItemService(IFortigateItemRepositories repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FortigateItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<FortigateItem>> GetActiveAsync()
        {
            return await _repository.GetActiveAsync();
        }

        public async Task<IEnumerable<FortigateItem>> GetByCategoryAsync(int fortigateCategoryId)
        {
            return await _repository.GetByCategoryAsync(fortigateCategoryId);
        }

        public async Task<FortigateItem> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(FortigateItem model)
        {
            if (model == null)
            {
                return "Invalid request.";
            }

            if (model.FortigateCategoryId <= 0)
            {
                return "Please select category.";
            }

            if (string.IsNullOrWhiteSpace(model.ItemName))
            {
                return "Item name is required.";
            }

            int result = await _repository.AddAsync(model);

            if (result > 0)
            {
                return "Fortigate item added successfully.";
            }

            return "Failed to add fortigate item.";
        }

        public async Task<string> UpdateAsync(FortigateItem model)
        {
            if (model == null)
            {
                return "Invalid request.";
            }

            if (model.FortigateItemId <= 0)
            {
                return "Invalid fortigate item id.";
            }

            if (model.FortigateCategoryId <= 0)
            {
                return "Please select category.";
            }

            if (string.IsNullOrWhiteSpace(model.ItemName))
            {
                return "Item name is required.";
            }

            int result = await _repository.UpdateAsync(model);

            if (result > 0)
            {
                return "Fortigate item updated successfully.";
            }

            return "Failed to update fortigate item.";
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid fortigate item id.";
            }

            int result = await _repository.DeleteAsync(id);

            if (result > 0)
            {
                return "Fortigate item deleted successfully.";
            }

            return "Failed to delete fortigate item.";
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid fortigate item id.";
            }

            int result = await _repository.ToggleAsync(id);

            if (result > 0)
            {
                return "Fortigate item status changed successfully.";
            }

            return "Failed to change fortigate item status.";
        }
    }
}