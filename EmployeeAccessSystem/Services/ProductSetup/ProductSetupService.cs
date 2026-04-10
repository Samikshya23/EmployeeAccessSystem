using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public class ProductSetupService : IProductSetupService
    {
        private readonly IProductSetupRepositories _repo;

        public ProductSetupService(IProductSetupRepositories repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProductSetup>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<ProductSetup> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(ProductSetup productSetup)
        {
            if (productSetup == null)
            {
                return "Invalid request.";
            }

            if (string.IsNullOrWhiteSpace(productSetup.ProductName))
            {
                return "Product Name is required.";
            }

            productSetup.ProductName = productSetup.ProductName.Trim();

            if (productSetup.ProductName.Length > 100)
            {
                return "Product Name cannot exceed 100 characters.";
            }

            try
            {
                int result = await _repo.AddAsync(productSetup);

                if (result > 0)
                {
                    return "Product added successfully.";
                }

                return "Failed to add product.";
            }
            catch (SqlException)
            {
                return "Database error while adding product.";
            }
            catch
            {
                return "Error while adding product.";
            }
        }

        public async Task<string> UpdateAsync(ProductSetup productSetup)
        {
            if (productSetup == null)
            {
                return "Invalid request.";
            }

            if (productSetup.ProductId <= 0)
            {
                return "Invalid product.";
            }

            if (string.IsNullOrWhiteSpace(productSetup.ProductName))
            {
                return "Product Name is required.";
            }

            productSetup.ProductName = productSetup.ProductName.Trim();

            if (productSetup.ProductName.Length > 100)
            {
                return "Product Name cannot exceed 100 characters.";
            }

            ProductSetup existing = await _repo.GetByIdAsync(productSetup.ProductId);

            if (existing == null)
            {
                return "Product not found.";
            }

            try
            {
                int result = await _repo.UpdateAsync(productSetup);

                if (result > 0)
                {
                    return "Product updated successfully.";
                }

                return "Failed to update product.";
            }
            catch (SqlException)
            {
                return "Database error while updating product.";
            }
            catch
            {
                return "Error while updating product.";
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid product.";
            }

            ProductSetup existing = await _repo.GetByIdAsync(id);

            if (existing == null)
            {
                return "Product not found.";
            }

            try
            {
                int result = await _repo.DeleteAsync(id);

                if (result > 0)
                {
                    return "Product deleted successfully.";
                }

                return "Failed to delete product.";
            }
            catch
            {
                return "This product cannot be deleted because it is used somewhere.";
            }
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid product.";
            }

            ProductSetup existing = await _repo.GetByIdAsync(id);

            if (existing == null)
            {
                return "Product not found.";
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