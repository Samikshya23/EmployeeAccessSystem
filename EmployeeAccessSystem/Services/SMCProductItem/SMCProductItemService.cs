using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

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
            if (model.SMCProductId <= 0)
            {
                return "Please select SMC Product.";
            }

            if (string.IsNullOrWhiteSpace(model.ItemName))
            {
                return "Item Name is required.";
            }

            //if (string.IsNullOrWhiteSpace(model.ValueType))
            //{
            //    return "Value Type is required.";
            //}

            await _repo.AddAsync(model);
            return null;
        }

        public async Task<string> UpdateAsync(SMCProductItem model)
        {
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

            //if (string.IsNullOrWhiteSpace(model.ValueType))
            //{
            //    return "Value Type is required.";
            //}

            await _repo.UpdateAsync(model);
            return null;
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid item.";
            }

            await _repo.DeleteAsync(id);
            return null;
        }

        public async Task ToggleAsync(int id)
        {
            await _repo.ToggleAsync(id);
        }
    }
}