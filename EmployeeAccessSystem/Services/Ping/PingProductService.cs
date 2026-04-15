using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Services
{
    public class PingProductService : IPingProductService
    {
        private readonly IPingProductRepository _repository;

        public PingProductService(IPingProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PingProduct>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<PingProduct>> GetActiveAsync()
        {
            return await _repository.GetActiveAsync();
        }

        public async Task<PingProduct> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(PingProduct pingProduct)
        {
            if (pingProduct == null)
            {
                return "Invalid request.";
            }

            if (pingProduct.ProductId <= 0)
            {
                return "Please select a product.";
            }

            if (string.IsNullOrWhiteSpace(pingProduct.PingProductName))
            {
                return "Please enter Ping product name.";
            }

            pingProduct.PingProductName = pingProduct.PingProductName.Trim();

            if (pingProduct.PingProductName.Length > 100)
            {
                return "Ping product name cannot exceed 100 characters.";
            }

            PingProduct duplicate = await _repository.CheckDuplicateAsync(
                pingProduct.ProductId,
                pingProduct.PingProductName
            );

            if (duplicate != null)
            {
                return "Ping Product name already exists for this product.";
            }

            try
            {
                int result = await _repository.AddAsync(pingProduct);

                if (result > 0)
                {
                    return "Ping Product added successfully.";
                }

                return "Failed to add Ping Product.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "Ping Product name already exists for this product.";
                }

                return "Database error while adding Ping Product.";
            }
            catch
            {
                return "Error while adding Ping Product.";
            }
        }

        public async Task<string> UpdateAsync(PingProduct pingProduct)
        {
            if (pingProduct == null)
            {
                return "Invalid request.";
            }

            if (pingProduct.PingProductId <= 0)
            {
                return "Invalid Ping Product.";
            }

            if (pingProduct.ProductId <= 0)
            {
                return "Please select a product.";
            }

            if (string.IsNullOrWhiteSpace(pingProduct.PingProductName))
            {
                return "Please enter Ping product name.";
            }

            pingProduct.PingProductName = pingProduct.PingProductName.Trim();

            if (pingProduct.PingProductName.Length > 100)
            {
                return "Ping product name cannot exceed 100 characters.";
            }

            PingProduct existing = await _repository.GetByIdAsync(pingProduct.PingProductId);

            if (existing == null)
            {
                return "Ping Product not found.";
            }

            PingProduct duplicate = await _repository.CheckDuplicateForUpdateAsync(
                pingProduct.PingProductId,
                pingProduct.ProductId,
                pingProduct.PingProductName
            );

            if (duplicate != null)
            {
                return "Ping Product name already exists for this product.";
            }

            try
            {
                int result = await _repository.UpdateAsync(pingProduct);

                if (result > 0)
                {
                    return "Ping Product updated successfully.";
                }

                return "Failed to update Ping Product.";
            }
            catch (SqlException ex)
            {
                if (ex.Message.Contains("String or binary data would be truncated"))
                {
                    return "Data is too long for database column.";
                }

                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "Ping Product name already exists for this product.";
                }

                return "Database error while updating Ping Product.";
            }
            catch
            {
                return "Error while updating Ping Product.";
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid Ping Product.";
            }

            PingProduct existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "Ping Product not found.";
            }

            try
            {
                int result = await _repository.DeleteAsync(id);

                if (result > 0)
                {
                    return "Ping Product deleted successfully.";
                }

                return "Failed to delete Ping Product.";
            }
            catch
            {
                return "This Ping Product cannot be deleted because it is used somewhere.";
            }
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid Ping Product.";
            }

            PingProduct existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "Ping Product not found.";
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