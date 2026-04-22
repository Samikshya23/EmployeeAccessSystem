using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _repository;

        public ConfigurationService(IConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Configuration>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Configuration>> GetByProductIdAsync(int productId)
        {
            return await _repository.GetByProductIdAsync(productId);
        }

        public async Task<Configuration> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<string> AddAsync(Configuration model)
        {
            if (model.ProductId <= 0)
            {
                return "Please select product.";
            }

            if (string.IsNullOrWhiteSpace(model.ConfigurationName))
            {
                return "Configuration name is required.";
            }

            if (string.IsNullOrWhiteSpace(model.NodeType))
            {
                return "Node type is required.";
            }

            if (model.NodeType == "Field" && string.IsNullOrWhiteSpace(model.InputType))
            {
                return "Input type is required for field node.";
            }

            int result = await _repository.AddAsync(model);

            if (result > 0)
            {
                return "Configuration added successfully.";
            }

            return "Failed to add configuration.";
        }

        public async Task<string> UpdateAsync(Configuration model)
        {
            if (model.ConfigurationId <= 0)
            {
                return "Invalid configuration id.";
            }

            if (model.ProductId <= 0)
            {
                return "Please select product.";
            }

            if (string.IsNullOrWhiteSpace(model.ConfigurationName))
            {
                return "Configuration name is required.";
            }

            if (string.IsNullOrWhiteSpace(model.NodeType))
            {
                return "Node type is required.";
            }

            if (model.NodeType == "Field" && string.IsNullOrWhiteSpace(model.InputType))
            {
                return "Input type is required for field node.";
            }

            int result = await _repository.UpdateAsync(model);

            if (result > 0)
            {
                return "Configuration updated successfully.";
            }

            return "Failed to update configuration.";
        }

        public async Task<string> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid configuration id.";
            }

            int result = await _repository.DeleteAsync(id);

            if (result > 0)
            {
                return "Configuration deleted successfully.";
            }

            return "Failed to delete configuration.";
        }

        public async Task<string> ToggleAsync(int id)
        {
            if (id <= 0)
            {
                return "Invalid configuration id.";
            }

            int result = await _repository.ToggleAsync(id);

            if (result > 0)
            {
                return "Configuration status changed successfully.";
            }

            return "Failed to change configuration status.";
        }
    }
}