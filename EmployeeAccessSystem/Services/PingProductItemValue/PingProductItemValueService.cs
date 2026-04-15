using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Services
{
    public class PingProductItemValueService : IPingProductItemValueService
    {
        private readonly IPingProductItemValueRepository _repository;

        public PingProductItemValueService(IPingProductItemValueRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PingProductItemValue>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<PingProductItemValue>> GetByItemIdAsync(int pingProductItemId)
        {
            return await _repository.GetByItemIdAsync(pingProductItemId);
        }

        public async Task<PingProductItemValue> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(PingProductItemValue pingProductItemValue)
        {
            if (pingProductItemValue == null)
            {
                return "Invalid request.";
            }

            if (pingProductItemValue.PingProductItemId <= 0)
            {
                return "Please select Ping Product Item.";
            }

            if (pingProductItemValue.PingProductFieldId <= 0)
            {
                return "Please select Ping Product Field.";
            }

            if (string.IsNullOrWhiteSpace(pingProductItemValue.FieldValue))
            {
                return "Please enter field value.";
            }

            pingProductItemValue.FieldValue = pingProductItemValue.FieldValue.Trim();

            if (pingProductItemValue.FieldValue.Length > 500)
            {
                return "Field value cannot exceed 500 characters.";
            }

            PingProductItemValue duplicate = await _repository.CheckDuplicateAsync(
                pingProductItemValue.PingProductItemId,
                pingProductItemValue.PingProductFieldId
            );

            if (duplicate != null)
            {
                return "Value already exists for this field in the selected item.";
            }

            try
            {
                int result = await _repository.AddAsync(pingProductItemValue);

                if (result > 0)
                {
                    return "Ping Product Item Value added successfully.";
                }

                return "Failed to add Ping Product Item Value.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }

                return "Database error while adding Ping Product Item Value.";
            }
            catch
            {
                return "Error while adding Ping Product Item Value.";
            }
        }

        public async Task<string> UpdateAsync(PingProductItemValue pingProductItemValue)
        {
            if (pingProductItemValue == null)
            {
                return "Invalid request.";
            }

            if (pingProductItemValue.PingProductItemValueId <= 0)
            {
                return "Invalid Ping Product Item Value.";
            }

            if (string.IsNullOrWhiteSpace(pingProductItemValue.FieldValue))
            {
                return "Please enter field value.";
            }

            pingProductItemValue.FieldValue = pingProductItemValue.FieldValue.Trim();

            if (pingProductItemValue.FieldValue.Length > 500)
            {
                return "Field value cannot exceed 500 characters.";
            }

            PingProductItemValue existing = await _repository.GetByIdAsync(pingProductItemValue.PingProductItemValueId);

            if (existing == null)
            {
                return "Ping Product Item Value not found.";
            }

            try
            {
                int result = await _repository.UpdateAsync(pingProductItemValue);

                if (result > 0)
                {
                    return "Ping Product Item Value updated successfully.";
                }

                return "Failed to update Ping Product Item Value.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }

                return "Database error while updating Ping Product Item Value.";
            }
            catch
            {
                return "Error while updating Ping Product Item Value.";
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid Ping Product Item Value.";
            }

            PingProductItemValue existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "Ping Product Item Value not found.";
            }

            try
            {
                int result = await _repository.DeleteAsync(id);

                if (result > 0)
                {
                    return "Ping Product Item Value deleted successfully.";
                }

                return "Failed to delete Ping Product Item Value.";
            }
            catch
            {
                return "This Ping Product Item Value cannot be deleted because it is used somewhere.";
            }
        }
    }
}