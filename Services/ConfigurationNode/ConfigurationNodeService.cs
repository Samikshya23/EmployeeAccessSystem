using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class ConfigurationNodeService : IConfigurationNodeService
    {
        private readonly IConfigurationNodeRepositories _repository;

        public ConfigurationNodeService(IConfigurationNodeRepositories repository)
        {
            _repository = repository;
        }
        public async Task<List<ConfigurationNode>> GetAllAsync()
        {
            IEnumerable<ConfigurationNode> data = await _repository.GetAllAsync();
            List<ConfigurationNode> allNodes = ConvertToList(data);
            List<ConfigurationNode> rootNodes = BuildTree(allNodes);
            return rootNodes;
        }
        public async Task<List<ConfigurationNode>> GetByProductIdAsync(int productId)
        {
            IEnumerable<ConfigurationNode> data = await _repository.GetByProductIdAsync(productId);
            List<ConfigurationNode> allNodes = ConvertToList(data);
            List<ConfigurationNode> rootNodes = BuildTree(allNodes);
            return rootNodes;
        }
        public async Task<List<ConfigurationNode>> GetFlatByProductIdAsync(int productId)
        {
            IEnumerable<ConfigurationNode> data = await _repository.GetByProductIdAsync(productId);
            List<ConfigurationNode> list = ConvertToList(data);
            SortNodeList(list);
            return list;
        }
        public async Task<ConfigurationNode> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async Task<(bool Success, string Message)> AddAsync(ConfigurationNode model)
        {
            if (model == null)
            {
                return (false, "Invalid node data.");
            }

            if (model.ProductId <= 0)
            {
                return (false, "Please select a product.");
            }

            if (string.IsNullOrWhiteSpace(model.NodeName))
            {
                return (false, "Node name is required.");
            }

            if (string.IsNullOrWhiteSpace(model.NodeType))
            {
                return (false, "Node type is required.");
            }

            if (model.NodeType != "Group" && model.NodeType != "Field")
            {
                return (false, "Invalid node type.");
            }

            if (model.NodeType == "Field")
            {
                if (string.IsNullOrWhiteSpace(model.InputType))
                {
                    return (false, "Input type is required for field node.");
                }
            }
            else
            {
                model.InputType = null;
            }

            if (model.SortOrder <= 0)
            {
                model.SortOrder = 1;
            }

            model.NodeName = model.NodeName.Trim();

            int result = await _repository.AddAsync(model);

            if (result > 0)
            {
                return (true, "Configuration node added successfully.");
            }

            return (false, "Failed to add configuration node.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(ConfigurationNode model)
        {
            if (model == null)
            {
                return (false, "Invalid node data.");
            }

            if (model.NodeId <= 0)
            {
                return (false, "Invalid node id.");
            }

            if (model.ProductId <= 0)
            {
                return (false, "Please select a product.");
            }

            if (string.IsNullOrWhiteSpace(model.NodeName))
            {
                return (false, "Node name is required.");
            }

            if (string.IsNullOrWhiteSpace(model.NodeType))
            {
                return (false, "Node type is required.");
            }

            if (model.NodeType != "Group" && model.NodeType != "Field")
            {
                return (false, "Invalid node type.");
            }

            if (model.ParentNodeId.HasValue && model.ParentNodeId.Value == model.NodeId)
            {
                return (false, "Node cannot be its own parent.");
            }

            if (model.NodeType == "Field")
            {
                if (string.IsNullOrWhiteSpace(model.InputType))
                {
                    return (false, "Input type is required for field node.");
                }
            }
            else
            {
                model.InputType = null;
            }

            model.NodeName = model.NodeName.Trim();

            int result = await _repository.UpdateAsync(model);

            if (result > 0)
            {
                return (true, "Configuration node updated successfully.");
            }

            return (false, "Failed to update configuration node.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                return (false, "Invalid node id.");
            }

            ConfigurationNode existingNode = await _repository.GetByIdAsync(id);

            if (existingNode == null)
            {
                return (false, "Node not found.");
            }

            int result = await _repository.DeleteAsync(id);

            if (result > 0)
            {
                return (true, "Configuration node deleted successfully.");
            }

            return (false, "Failed to delete configuration node.");
        }

        private List<ConfigurationNode> ConvertToList(IEnumerable<ConfigurationNode> data)
        {
            List<ConfigurationNode> list = new List<ConfigurationNode>();

            if (data != null)
            {
                foreach (ConfigurationNode item in data)
                {
                    item.Children = new List<ConfigurationNode>();
                    list.Add(item);
                }
            }

            return list;
        }

        private List<ConfigurationNode> BuildTree(List<ConfigurationNode> allNodes)
        {
            List<ConfigurationNode> rootNodes = new List<ConfigurationNode>();

            foreach (ConfigurationNode node in allNodes)
            {
                if (node.ParentNodeId == null)
                {
                    rootNodes.Add(node);
                }
            }

            SortNodeList(rootNodes);

            foreach (ConfigurationNode rootNode in rootNodes)
            {
                AddChildren(rootNode, allNodes);
            }

            return rootNodes;
        }

        private void AddChildren(ConfigurationNode parentNode, List<ConfigurationNode> allNodes)
        {
            List<ConfigurationNode> childNodes = new List<ConfigurationNode>();

            foreach (ConfigurationNode node in allNodes)
            {
                if (node.ParentNodeId == parentNode.NodeId)
                {
                    childNodes.Add(node);
                }
            }

            SortNodeList(childNodes);

            foreach (ConfigurationNode childNode in childNodes)
            {
                parentNode.Children.Add(childNode);
                AddChildren(childNode, allNodes);
            }
        }

        private void SortNodeList(List<ConfigurationNode> nodes)
        {
            nodes.Sort(delegate (ConfigurationNode a, ConfigurationNode b)
            {
                int result = a.SortOrder.CompareTo(b.SortOrder);

                if (result == 0)
                {
                    result = a.NodeId.CompareTo(b.NodeId);
                }

                return result;
            });
        }
    }
}