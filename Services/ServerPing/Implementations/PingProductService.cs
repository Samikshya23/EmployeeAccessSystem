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
                return "Invalid request.";

            if (pingProduct.ProductId <= 0)
                return "Please select a product.";

            if (string.IsNullOrWhiteSpace(pingProduct.IPAddress))
                return "Please enter IP address.";

            if (string.IsNullOrWhiteSpace(pingProduct.ServerHostName))
                return "Please enter server host name.";

            pingProduct.IPAddress = pingProduct.IPAddress.Trim();
            pingProduct.ServerHostName = pingProduct.ServerHostName.Trim();

            PingProduct duplicate = await _repository.CheckDuplicateAsync(
                pingProduct.IPAddress,
                pingProduct.ServerHostName
            );

            if (duplicate != null)
                return "This IP Address and Server Host Name already exists.";

            try
            {
                int result = await _repository.AddAsync(pingProduct);
                return result > 0 ? "Ping Product added successfully." : "Failed to add Ping Product.";
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                    return "This IP Address and Server Host Name already exists.";

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
                return "Invalid request.";

            if (pingProduct.PingProductId <= 0)
                return "Invalid Ping Product.";

            if (pingProduct.ProductId <= 0)
                return "Please select a product.";

            if (string.IsNullOrWhiteSpace(pingProduct.IPAddress))
                return "Please enter IP address.";

            if (string.IsNullOrWhiteSpace(pingProduct.ServerHostName))
                return "Please enter server host name.";

            pingProduct.IPAddress = pingProduct.IPAddress.Trim();
            pingProduct.ServerHostName = pingProduct.ServerHostName.Trim();

            PingProduct existing = await _repository.GetByIdAsync(pingProduct.PingProductId);

            if (existing == null)
                return "Ping Product not found.";

            pingProduct.IsActive = existing.IsActive;

            PingProduct duplicate = await _repository.CheckDuplicateForUpdateAsync(
                pingProduct.PingProductId,
                pingProduct.IPAddress,
                pingProduct.ServerHostName
            );

            if (duplicate != null)
                return "This IP Address and Server Host Name already exists.";

            try
            {
                int result = await _repository.UpdateAsync(pingProduct);
                return result > 0 ? "Ping Product updated successfully." : "Failed to update Ping Product.";
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                    return "This IP Address and Server Host Name already exists.";

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
                return "Invalid Ping Product.";

            PingProduct existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return "Ping Product not found.";

            try
            {
                int result = await _repository.DeleteAsync(id);
                return result > 0 ? "Ping Product deleted successfully." : "Failed to delete Ping Product.";
            }
            catch
            {
                return "This Ping Product cannot be deleted because it is used somewhere.";
            }
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
                return "Invalid Ping Product.";

            PingProduct existing = await _repository.GetByIdAsync(id);

            if (existing == null)
                return "Ping Product not found.";

            try
            {
                int result = await _repository.ToggleAsync(id);
                return result > 0 ? "Status changed successfully." : "Failed to change status.";
            }
            catch
            {
                return "Error while changing status.";
            }
        }
    }
}