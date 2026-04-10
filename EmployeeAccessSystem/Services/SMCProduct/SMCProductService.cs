using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.Data.SqlClient;
namespace EmployeeAccessSystem.Services
{
    public class SMCProductService : ISMCProductService
    {
        private readonly ISMCProductRepository _repository;

        public SMCProductService(ISMCProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<SMCProduct>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<IEnumerable<SMCProduct>> GetActiveAsync()
        {
            return await _repository.GetActiveAsync();
        }
        public async Task<SMCProduct> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<string> AddAsync(SMCProduct smcProduct)
        {
            if (smcProduct == null)
            {
                return "Invalid request.";
            }
            if (smcProduct.ProductId <= 0)
            {
                return "Please select a product.";
            }
            if (string.IsNullOrWhiteSpace(smcProduct.SMCProductName))
            {
                return "Please enter SMC product name.";
            }
            smcProduct.SMCProductName = smcProduct.SMCProductName.Trim();

            if (smcProduct.SMCProductName.Length > 100)
            {
                return "SMC product name cannot exceed 100 characters.";
            }
            SMCProduct duplicate = await _repository.CheckDuplicateAsync(smcProduct.ProductId,smcProduct.SMCProductName);

            if (duplicate != null)
            {
                return "SMC Product name already exists for this product.";
            }
            try
            {
                int result = await _repository.AddAsync(smcProduct);

                if (result > 0)
                {
                    return "SMC Product added successfully.";
                }

                return "Failed to add SMC Product.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "SMC Product name already exists for this product.";
                }
                return "Database error while adding SMC Product.";
            }
            catch
            {
                return "Error while adding SMC Product.";
            }
        }
        public async Task<string> UpdateAsync(SMCProduct smcProduct)
        {
            if (smcProduct == null)
            {
                return "Invalid request.";
            }
            if (smcProduct.SMCProductId <= 0)
            {
                return "Invalid SMC Product.";
            }
            if (smcProduct.ProductId <= 0)
            {
                return "Please select a product.";
            }
            if (string.IsNullOrWhiteSpace(smcProduct.SMCProductName))
            {
                return "Please enter SMC product name.";
            }
            smcProduct.SMCProductName = smcProduct.SMCProductName.Trim();

            if (smcProduct.SMCProductName.Length > 100)
            {
                return "SMC product name cannot exceed 100 characters.";
            }

            SMCProduct existing = await _repository.GetByIdAsync(smcProduct.SMCProductId);

            if (existing == null)
            {
                return "SMC Product not found.";
            }

            SMCProduct duplicate = await _repository.CheckDuplicateForUpdateAsync(
                smcProduct.SMCProductId,
                smcProduct.ProductId,
                smcProduct.SMCProductName
            );

            if (duplicate != null)
            {
                return "SMC Product name already exists for this product.";
            }

            try
            {
                int result = await _repository.UpdateAsync(smcProduct);

                if (result > 0)
                {
                    return "SMC Product updated successfully.";
                }

                return "Failed to update SMC Product.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "SMC Product name already exists for this product.";
                }

                return "Database error while updating SMC Product.";
            }
            catch
            {
                return "Error while updating SMC Product.";
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid SMC Product.";
            }

            SMCProduct existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "SMC Product not found.";
            }

            try
            {
                int result = await _repository.DeleteAsync(id);

                if (result > 0)
                {
                    return "SMC Product deleted successfully.";
                }

                return "Failed to delete SMC Product.";
            }
            catch
            {
                return "This SMC Product cannot be deleted because it is used somewhere.";
            }
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid SMC Product.";
            }

            SMCProduct existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "SMC Product not found.";
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