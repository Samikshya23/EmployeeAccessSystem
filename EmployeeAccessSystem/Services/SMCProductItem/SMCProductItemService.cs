using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Services
{
    public class SMCProductItemService : ISMCProductItemService
    {
        private readonly ISMCProductItemRepositories _repo;

        public SMCProductItemService(ISMCProductItemRepositories repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SMCProductItem>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<IEnumerable<SMCProductItem>> GetByProductAsync(int smcProductId)
        {
            return await _repo.GetByProductAsync(smcProductId);
        }

        public async Task<SMCProductItem> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(SMCProductItem model)
        {
            if (model == null)
            {
                return "Invalid request.";
            }

            if (model.SMCProductId <= 0)
            {
                return "Please select SMC Product.";
            }

            if (string.IsNullOrWhiteSpace(model.ItemName))
            {
                return "Item Name is required.";
            }

            model.ItemName = model.ItemName.Trim();

            if (model.ItemName.Length > 200)
            {
                return "Item Name cannot exceed 200 characters.";
            }

            try
            {
                int result = await _repo.AddAsync(model);

                if (result > 0)
                {
                    return "SMC Product Item added successfully.";
                }

                return "Failed to add SMC Product Item.";
            }
            catch (SqlException)
            {
                return "Database error while adding SMC Product Item.";
            }
            catch
            {
                return "Error while adding SMC Product Item.";
            }
        }

        public async Task<string> UpdateAsync(SMCProductItem model)
        {
            if (model == null)
            {
                return "Invalid request.";
            }

            if (model.SMCProductItemId <= 0)
            {
                return "Invalid item.";
            }

            if (model.SMCProductId <= 0)
            {
                return "Please select SMC Product.";
            }

            if (string.IsNullOrWhiteSpace(model.ItemName))
            {
                return "Item Name is required.";
            }

            model.ItemName = model.ItemName.Trim();

            if (model.ItemName.Length > 200)
            {
                return "Item Name cannot exceed 200 characters.";
            }

            SMCProductItem existingItem = await _repo.GetByIdAsync(model.SMCProductItemId);

            if (existingItem == null)
            {
                return "SMC Product Item not found.";
            }

            try
            {
                int result = await _repo.UpdateAsync(model);

                if (result > 0)
                {
                    return "SMC Product Item updated successfully.";
                }

                return "Failed to update SMC Product Item.";
            }
            catch (SqlException)
            {
                return "Database error while updating SMC Product Item.";
            }
            catch
            {
                return "Error while updating SMC Product Item.";
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid item.";
            }

            SMCProductItem existingItem = await _repo.GetByIdAsync(id);

            if (existingItem == null)
            {
                return "SMC Product Item not found.";
            }

            try
            {
                int result = await _repo.DeleteAsync(id);

                if (result > 0)
                {
                    return "SMC Product Item deleted successfully.";
                }

                return "Failed to delete SMC Product Item.";
            }
            catch
            {
                return "Already used in config.";
            }
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid item.";
            }

            SMCProductItem existingItem = await _repo.GetByIdAsync(id);

            if (existingItem == null)
            {
                return "SMC Product Item not found.";
            }

            try
            {
                int result = await _repo.ToggleAsync(id);

                if (result > 0)
                {
                    return "Status changed successfully.";
                }

                return "Failed to change status.";
            }
            catch
            {
                return "Error while changing status.";
            }
        }
    }
}