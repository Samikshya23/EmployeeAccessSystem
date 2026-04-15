using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Services
{
    public class PingProductItemService : IPingProductItemService
    {
        private readonly IPingProductItemRepository _repository;

        public PingProductItemService(IPingProductItemRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PingProductItem>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<PingProductItem>> GetActiveAsync()
        {
            return await _repository.GetActiveAsync();
        }

        public async Task<PingProductItem> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(PingProductItem pingProductItem)
        {
            if (pingProductItem == null)
            {
                return "Invalid request.";
            }

            if (pingProductItem.PingProductId <= 0)
            {
                return "Please select Ping Product.";
            }

            if (string.IsNullOrWhiteSpace(pingProductItem.ItemName))
            {
                return "Please enter item name.";
            }

            pingProductItem.ItemName = pingProductItem.ItemName.Trim();

            if (pingProductItem.ItemName.Length > 100)
            {
                return "Item name cannot exceed 100 characters.";
            }

            PingProductItem duplicate = await _repository.CheckDuplicateAsync(
                pingProductItem.PingProductId,
                pingProductItem.ItemName
            );

            if (duplicate != null)
            {
                return "Item name already exists for this Ping Product.";
            }

            try
            {
                int result = await _repository.AddAsync(pingProductItem);

                if (result > 0)
                {
                    return "Ping Product Item added successfully.";
                }

                return "Failed to add Ping Product Item.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "Item name already exists for this Ping Product.";
                }

                return "Database error while adding Ping Product Item.";
            }
            catch
            {
                return "Error while adding Ping Product Item.";
            }
        }

        public async Task<string> UpdateAsync(PingProductItem pingProductItem)
        {
            if (pingProductItem == null)
            {
                return "Invalid request.";
            }

            if (pingProductItem.PingProductItemId <= 0)
            {
                return "Invalid Ping Product Item.";
            }

            if (pingProductItem.PingProductId <= 0)
            {
                return "Please select Ping Product.";
            }

            if (string.IsNullOrWhiteSpace(pingProductItem.ItemName))
            {
                return "Please enter item name.";
            }

            pingProductItem.ItemName = pingProductItem.ItemName.Trim();

            if (pingProductItem.ItemName.Length > 100)
            {
                return "Item name cannot exceed 100 characters.";
            }

            PingProductItem existing = await _repository.GetByIdAsync(pingProductItem.PingProductItemId);

            if (existing == null)
            {
                return "Ping Product Item not found.";
            }

            PingProductItem duplicate = await _repository.CheckDuplicateForUpdateAsync(
                pingProductItem.PingProductItemId,
                pingProductItem.PingProductId,
                pingProductItem.ItemName
            );

            if (duplicate != null)
            {
                return "Item name already exists for this Ping Product.";
            }

            try
            {
                int result = await _repository.UpdateAsync(pingProductItem);

                if (result > 0)
                {
                    return "Ping Product Item updated successfully.";
                }

                return "Failed to update Ping Product Item.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "Item name already exists for this Ping Product.";
                }

                return "Database error while updating Ping Product Item.";
            }
            catch
            {
                return "Error while updating Ping Product Item.";
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid Ping Product Item.";
            }

            PingProductItem existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "Ping Product Item not found.";
            }

            try
            {
                int result = await _repository.DeleteAsync(id);

                if (result > 0)
                {
                    return "Ping Product Item deleted successfully.";
                }

                return "Failed to delete Ping Product Item.";
            }
            catch
            {
                return "This Ping Product Item cannot be deleted because it is used somewhere.";
            }
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid Ping Product Item.";
            }

            PingProductItem existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "Ping Product Item not found.";
            }

            try
            {
                int result = await _repository.ToggleAsync(id);

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