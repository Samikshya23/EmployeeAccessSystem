using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class FortigateConfigService : IFortigateConfigService
    {
        private readonly IFortigateConfigRepositories _repo;

        public FortigateConfigService(IFortigateConfigRepositories repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<FortigateConfig>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<FortigateConfig?> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<FortigateConfig?> GetExistingAsync(int productId, int categoryId, int itemId, DateTime date)
        {
            return await _repo.GetExistingAsync(productId, categoryId, itemId, date);
        }

        public async Task<string> AddAsync(FortigateConfig model, string? currentUser)
        {
            try
            {
                string validation = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(validation))
                {
                    return "error|" + validation;
                }

                model.EntryDate = model.EntryDate.Date;

                var existing = await _repo.GetExistingAsync(
                    model.ProductId,
                    model.FortigateCategoryId,
                    model.FortigateItemId,
                    model.EntryDate
                );

                if (existing != null)
                {
                    return "error|Configuration already exists for this date.";
                }

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = currentUser;

                int result = await _repo.AddAsync(model);

                if (result > 0)
                {
                    return "success|Saved successfully.";
                }

                return "error|Failed to save.";
            }
            catch
            {
                return "error|Database error occurred.";
            }
        }

        public async Task<string> UpdateAsync(FortigateConfig model, string? currentUser)
        {
            try
            {
                if (model.FortigateConfigId <= 0)
                {
                    return "error|Invalid ID.";
                }

                string validation = ValidateModel(model);
                if (!string.IsNullOrWhiteSpace(validation))
                {
                    return "error|" + validation;
                }

                model.EntryDate = model.EntryDate.Date;

                var existing = await _repo.GetExistingAsync(
                    model.ProductId,
                    model.FortigateCategoryId,
                    model.FortigateItemId,
                    model.EntryDate
                );

                if (existing != null && existing.FortigateConfigId != model.FortigateConfigId)
                {
                    return "error|Configuration already exists for this date.";
                }

                model.ModifiedDate = DateTime.Now;
                model.ModifiedBy = currentUser;

                int result = await _repo.UpdateAsync(model);

                if (result > 0)
                {
                    return "success|Updated successfully.";
                }

                return "error|Failed to update.";
            }
            catch
            {
                return "error|Database error occurred.";
            }
        }

        public async Task<string> DeleteAsync(int id, string? currentUser)
        {
            try
            {
                if (id <= 0)
                {
                    return "error|Invalid ID.";
                }

                var existing = await _repo.GetByIdAsync(id);
                if (existing == null)
                {
                    return "error|Not found.";
                }

                int result = await _repo.DeleteAsync(id, DateTime.Now, currentUser);

                if (result > 0)
                {
                    return "success|Deleted successfully.";
                }

                return "error|Delete failed.";
            }
            catch
            {
                return "error|Database error occurred.";
            }
        }

        public async Task<string> ToggleAsync(int id)
        {
            try
            {
                int result = await _repo.ToggleAsync(id);

                if (result > 0)
                {
                    return "success|Status updated.";
                }

                return "error|Failed to update.";
            }
            catch
            {
                return "error|Database error occurred.";
            }
        }

        private string ValidateModel(FortigateConfig model)
        {
            if (model.ProductId <= 0)
                return "Select Product.";

            if (model.FortigateCategoryId <= 0)
                return "Select Category.";

            if (model.FortigateItemId <= 0)
                return "Select Item.";

            if (string.IsNullOrWhiteSpace(model.ConfigValue))
                return "Enter status (UP/DOWN).";

            model.ConfigValue = model.ConfigValue.Trim().ToUpper();

            return string.Empty;
        }
    }
}