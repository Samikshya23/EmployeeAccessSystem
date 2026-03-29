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
            if (string.IsNullOrWhiteSpace(smcProduct.SMCProductName))
            {
                return "SMC Product Name is required.";
            }

            await _repository.AddAsync(smcProduct);
            return null;
        }

        public async Task<string> UpdateAsync(SMCProduct smcProduct)
        {
            if (string.IsNullOrWhiteSpace(smcProduct.SMCProductName))
            {
                return "SMC Product Name is required.";
            }

            await _repository.UpdateAsync(smcProduct);
            return null;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task ToggleAsync(int id)
        {
            await _repository.ToggleAsync(id);
        }
    }
}