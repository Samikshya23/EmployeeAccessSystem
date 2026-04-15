using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Services
{
    public class PingProductFieldService : IPingProductFieldService
    {
        private readonly IPingProductFieldRepository _repository;

        public PingProductFieldService(IPingProductFieldRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PingProductField>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<PingProductField>> GetActiveAsync()
        {
            return await _repository.GetActiveAsync();
        }

        public async Task<PingProductField> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(PingProductField pingProductField)
        {
            if (pingProductField == null)
            {
                return "Invalid request.";
            }

            if (pingProductField.PingProductId <= 0)
            {
                return "Please select Ping Product.";
            }

            if (string.IsNullOrWhiteSpace(pingProductField.FieldName))
            {
                return "Please enter field name.";
            }

            pingProductField.FieldName = pingProductField.FieldName.Trim();

            if (pingProductField.FieldName.Length > 100)
            {
                return "Field name cannot exceed 100 characters.";
            }

            if (pingProductField.DisplayOrder <= 0)
            {
                pingProductField.DisplayOrder = 1;
            }

            PingProductField duplicate = await _repository.CheckDuplicateAsync(
                pingProductField.PingProductId,
                pingProductField.FieldName
            );

            if (duplicate != null)
            {
                return "Field name already exists for this Ping Product.";
            }

            try
            {
                int result = await _repository.AddAsync(pingProductField);

                if (result > 0)
                {
                    return "Ping Product Field added successfully.";
                }

                return "Failed to add Ping Product Field.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "Field name already exists for this Ping Product.";
                }

                return "Database error while adding Ping Product Field.";
            }
            catch
            {
                return "Error while adding Ping Product Field.";
            }
        }

        public async Task<string> UpdateAsync(PingProductField pingProductField)
        {
            if (pingProductField == null)
            {
                return "Invalid request.";
            }

            if (pingProductField.PingProductFieldId <= 0)
            {
                return "Invalid Ping Product Field.";
            }

            if (pingProductField.PingProductId <= 0)
            {
                return "Please select Ping Product.";
            }

            if (string.IsNullOrWhiteSpace(pingProductField.FieldName))
            {
                return "Please enter field name.";
            }

            pingProductField.FieldName = pingProductField.FieldName.Trim();

            if (pingProductField.FieldName.Length > 100)
            {
                return "Field name cannot exceed 100 characters.";
            }

            if (pingProductField.DisplayOrder <= 0)
            {
                pingProductField.DisplayOrder = 1;
            }

            PingProductField existing = await _repository.GetByIdAsync(pingProductField.PingProductFieldId);

            if (existing == null)
            {
                return "Ping Product Field not found.";
            }

            PingProductField duplicate = await _repository.CheckDuplicateForUpdateAsync(
                pingProductField.PingProductFieldId,
                pingProductField.PingProductId,
                pingProductField.FieldName
            );

            if (duplicate != null)
            {
                return "Field name already exists for this Ping Product.";
            }

            try
            {
                int result = await _repository.UpdateAsync(pingProductField);

                if (result > 0)
                {
                    return "Ping Product Field updated successfully.";
                }

                return "Failed to update Ping Product Field.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "Field name already exists for this Ping Product.";
                }

                return "Database error while updating Ping Product Field.";
            }
            catch
            {
                return "Error while updating Ping Product Field.";
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid Ping Product Field.";
            }

            PingProductField existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "Ping Product Field not found.";
            }

            try
            {
                int result = await _repository.DeleteAsync(id);

                if (result > 0)
                {
                    return "Ping Product Field deleted successfully.";
                }

                return "Failed to delete Ping Product Field.";
            }
            catch
            {
                return "This Ping Product Field cannot be deleted because it is used somewhere.";
            }
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid Ping Product Field.";
            }

            PingProductField existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "Ping Product Field not found.";
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