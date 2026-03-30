using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

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
            if (smcProduct.ProductId <= 0)
            {
                return "Product is required.";
            }

            if (string.IsNullOrWhiteSpace(smcProduct.SMCProductName))
            {
                return "SMC Product Name is required.";
            }

            await _repository.AddAsync(smcProduct);
            return null;
        }

        public async Task<string> UpdateAsync(SMCProduct smcProduct)
        {
            if (smcProduct.ProductId <= 0)
            {
                return "Product is required.";
            }

            if (string.IsNullOrWhiteSpace(smcProduct.SMCProductName))
            {
                return "SMC Product Name is required.";
            }

            await _repository.UpdateAsync(smcProduct);
            return null;
        }

        public async Task<string> DeleteAsync(int id)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);

                if (existing == null)
                {
                    return "SMC Product not found.";
                }

                await _repository.DeleteAsync(id);
                return null;
            }
            catch
            {
                return "This SMC Product cannot be deleted because related SMC Product Items exist. Please delete related items first or make this product inactive.";
            }
        }

        public async Task ToggleAsync(int id)
        {
            await _repository.ToggleAsync(id);
        }
    }
}