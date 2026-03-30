using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class SMCConfigService : ISMCConfigService
    {
        private readonly ISMCConfigRepository _repo;

        public SMCConfigService(ISMCConfigRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SMCConfig>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<SMCConfig?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<SMCConfig?> GetExistingAsync(int productId, int smcProductId, int itemId, DateTime date)
        {
            return await _repo.GetExistingAsync(productId, smcProductId, itemId, date);
        }

        public async Task<int> AddAsync(SMCConfig model)
        {
            if (model.ProductId <= 0 || model.SMCProductId <= 0 || model.SMCProductItemId <= 0)
            {
                return 0;
            }

            if (string.IsNullOrWhiteSpace(model.EntryMode))
            {
                return 0;
            }

            if (model.EntryMode == "Value")
            {
                if (string.IsNullOrWhiteSpace(model.ConfigValue))
                {
                    return 0;
                }

                model.ConfigValue = model.ConfigValue.Trim();
                model.IsChecked = false;
            }
            else if (model.EntryMode == "Checkbox")
            {
                model.ConfigValue = null;
            }
            else
            {
                return 0;
            }

            var existing = await _repo.GetExistingAsync(
                model.ProductId,
                model.SMCProductId,
                model.SMCProductItemId,
                model.EntryDate
            );

            if (existing != null)
            {
                model.SMCConfigId = existing.SMCConfigId;
                return await _repo.UpdateAsync(model);
            }

            return await _repo.AddAsync(model);
        }

        public async Task<int> UpdateAsync(SMCConfig model)
        {
            if (model.SMCConfigId <= 0 || model.ProductId <= 0 || model.SMCProductId <= 0 || model.SMCProductItemId <= 0)
            {
                return 0;
            }

            if (string.IsNullOrWhiteSpace(model.EntryMode))
            {
                return 0;
            }

            if (model.EntryMode == "Value")
            {
                if (string.IsNullOrWhiteSpace(model.ConfigValue))
                {
                    return 0;
                }

                model.ConfigValue = model.ConfigValue.Trim();
                model.IsChecked = false;
            }
            else if (model.EntryMode == "Checkbox")
            {
                model.ConfigValue = null;
            }
            else
            {
                return 0;
            }

            return await _repo.UpdateAsync(model);
        }

        public async Task<int> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return 0;
            }

            return await _repo.DeleteAsync(id);
        }

        public async Task<int> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return 0;
            }

            return await _repo.ToggleAsync(id);
        }
    }
}