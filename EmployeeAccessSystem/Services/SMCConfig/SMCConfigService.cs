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

        public async Task<string> AddAsync(SMCConfig model, string? currentUser)
        {
            try
            {
                string validationMessage = ValidateModel(model, false);
                if (!string.IsNullOrWhiteSpace(validationMessage))
                {
                    return "error|" + validationMessage;
                }

                model.EntryDate = model.EntryDate.Date;

                SMCConfig? existing = await _repo.GetExistingAsync(
                    model.ProductId,
                    model.SMCProductId,
                    model.SMCProductItemId,
                    model.EntryDate
                );

                if (existing != null)
                {
                    return "error|Configuration already exists for the selected date.";
                }

                model.CreatedDate = DateTime.Now;
                model.CreatedBy = currentUser;

                int result = await _repo.AddAsync(model);

                if (result > 0)
                {
                    return "success|Saved successfully.";
                }

                return "error|Data could not be saved.";
            }
            catch
            {
                return "error|Database error occurred.";
            }
        }

        public async Task<string> UpdateAsync(SMCConfig model, string? currentUser)
        {
            try
            {
                string validationMessage = ValidateModel(model, true);
                if (!string.IsNullOrWhiteSpace(validationMessage))
                {
                    return "error|" + validationMessage;
                }

                model.EntryDate = model.EntryDate.Date;

                SMCConfig? existing = await _repo.GetExistingAsync(
                    model.ProductId,
                    model.SMCProductId,
                    model.SMCProductItemId,
                    model.EntryDate
                );

                if (existing != null && existing.SMCConfigId != model.SMCConfigId)
                {
                    return "error|Configuration already exists for the selected date.";
                }

                model.ModifiedDate = DateTime.Now;
                model.ModifiedBy = currentUser;

                int result = await _repo.UpdateAsync(model);

                if (result > 0)
                {
                    return "success|Updated successfully.";
                }

                return "error|Data could not be updated.";
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
                    return "error|Invalid configuration id.";
                }

                SMCConfig? existing = await _repo.GetByIdAsync(id);

                if (existing == null)
                {
                    return "error|Configuration not found.";
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
                if (id <= 0)
                {
                    return "error|Invalid configuration id.";
                }

                SMCConfig? existing = await _repo.GetByIdAsync(id);

                if (existing == null)
                {
                    return "error|Configuration not found.";
                }

                int result = await _repo.ToggleAsync(id);

                if (result > 0)
                {
                    return "success|Status updated successfully.";
                }

                return "error|Status could not be updated.";
            }
            catch
            {
                return "error|Database error occurred.";
            }
        }

        private string ValidateModel(SMCConfig model, bool isUpdate)
        {
            if (isUpdate && model.SMCConfigId <= 0)
            {
                return "Invalid configuration id.";
            }

            if (model.ProductId <= 0)
            {
                return "Please select Product.";
            }

            if (model.SMCProductId <= 0)
            {
                return "Please select SMC Product.";
            }

            if (model.SMCProductItemId <= 0)
            {
                return "Please select SMC Product Item.";
            }

            if (string.IsNullOrWhiteSpace(model.EntryMode))
            {
                return "Please select Save Option.";
            }

            if (model.EntryMode != "Value" && model.EntryMode != "Checkbox")
            {
                return "Invalid Save Option.";
            }

            if (model.EntryMode == "Value")
            {
                if (string.IsNullOrWhiteSpace(model.ConfigValue))
                {
                    return "Please enter value.";
                }

                model.ConfigValue = model.ConfigValue.Trim();
                model.IsChecked = false;
            }
            else if (model.EntryMode == "Checkbox")
            {
                model.ConfigValue = null;
            }

            return string.Empty;
        }
    }
}