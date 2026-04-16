using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using Microsoft.Data.SqlClient;

namespace EmployeeAccessSystem.Services
{
    public class PingConfigService : IPingConfigService
    {
        private readonly IPingConfigRepository _repository;

        public PingConfigService(IPingConfigRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PingConfig>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<PingConfig?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return await _repository.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(PingConfig pingConfig, string currentUser)
        {
            if (pingConfig == null)
            {
                return "Invalid request.";
            }

            if (pingConfig.ProductId <= 0)
            {
                return "Please select a product.";
            }

            if (string.IsNullOrWhiteSpace(pingConfig.IPAddress))
            {
                return "Please select IP Address.";
            }

            if (pingConfig.PingProductId <= 0)
            {
                return "Please select server host name.";
            }

            if (pingConfig.EntryDate == DateTime.MinValue)
            {
                pingConfig.EntryDate = DateTime.Now;
            }

            if (string.IsNullOrWhiteSpace(pingConfig.EntryMode))
            {
                return "Please select save option.";
            }

            pingConfig.EntryMode = pingConfig.EntryMode.Trim();

            if (pingConfig.EntryMode == "Value")
            {
                if (string.IsNullOrWhiteSpace(pingConfig.ConfigValue))
                {
                    return "Please enter value.";
                }

                pingConfig.IsChecked = false;
            }
            else if (pingConfig.EntryMode == "Checkbox")
            {
                pingConfig.ConfigValue = null;
            }
            else
            {
                return "Invalid save option.";
            }

            PingConfig? duplicate = await _repository.CheckDuplicateAsync(
                pingConfig.PingProductId,
                pingConfig.EntryDate
            );

            if (duplicate != null)
            {
                return "Ping Config already exists for selected server and date.";
            }

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                currentUser = "System";
            }

            pingConfig.CreatedBy = currentUser;

            try
            {
                int result = await _repository.AddAsync(pingConfig);

                if (result > 0)
                {
                    return "Ping Config added successfully.";
                }

                return "Failed to add Ping Config.";
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "Ping Config already exists for selected server and date.";
                }

                return "Database error while adding Ping Config.";
            }
            catch
            {
                return "Error while adding Ping Config.";
            }
        }

        public async Task<string> UpdateAsync(PingConfig pingConfig, string currentUser)
        {
            if (pingConfig == null)
            {
                return "Invalid request.";
            }

            if (pingConfig.PingConfigId <= 0)
            {
                return "Invalid Ping Config.";
            }

            if (pingConfig.ProductId <= 0)
            {
                return "Please select a product.";
            }

            if (string.IsNullOrWhiteSpace(pingConfig.IPAddress))
            {
                return "Please select IP Address.";
            }

            if (pingConfig.PingProductId <= 0)
            {
                return "Please select server host name.";
            }

            if (pingConfig.EntryDate == DateTime.MinValue)
            {
                return "Invalid entry date.";
            }

            if (string.IsNullOrWhiteSpace(pingConfig.EntryMode))
            {
                return "Please select save option.";
            }

            pingConfig.EntryMode = pingConfig.EntryMode.Trim();

            if (pingConfig.EntryMode == "Value")
            {
                if (string.IsNullOrWhiteSpace(pingConfig.ConfigValue))
                {
                    return "Please enter value.";
                }

                pingConfig.IsChecked = false;
            }
            else if (pingConfig.EntryMode == "Checkbox")
            {
                pingConfig.ConfigValue = null;
            }
            else
            {
                return "Invalid save option.";
            }

            PingConfig? existing = await _repository.GetByIdAsync(pingConfig.PingConfigId);

            if (existing == null)
            {
                return "Ping Config not found.";
            }

            PingConfig? duplicate = await _repository.CheckDuplicateForUpdateAsync(
                pingConfig.PingConfigId,
                pingConfig.PingProductId,
                pingConfig.EntryDate
            );

            if (duplicate != null)
            {
                return "Ping Config already exists for selected server and date.";
            }

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                currentUser = "System";
            }

            pingConfig.ModifiedBy = currentUser;

            try
            {
                int result = await _repository.UpdateAsync(pingConfig);

                if (result > 0)
                {
                    return "Ping Config updated successfully.";
                }

                return "Failed to update Ping Config.";
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2601 || ex.Number == 2627)
                {
                    return "Ping Config already exists for selected server and date.";
                }

                return "Database error while updating Ping Config.";
            }
            catch
            {
                return "Error while updating Ping Config.";
            }
        }

        public async Task<string> DeleteAsync(int id, string currentUser)
        {
            if (id <= 0)
            {
                return "Invalid Ping Config.";
            }

            PingConfig? existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                return "Ping Config not found.";
            }

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                currentUser = "System";
            }

            try
            {
                int result = await _repository.DeleteAsync(id, currentUser);

                if (result > 0)
                {
                    return "Ping Config deleted successfully.";
                }

                return "Failed to delete Ping Config.";
            }
            catch
            {
                return "Error while deleting Ping Config.";
            }
        }
    }
}