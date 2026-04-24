using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class ProductConfigurationService : IProductConfigurationService
    {
        private readonly IProductConfigurationRepository _repository;

        public ProductConfigurationService(IProductConfigurationRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProductConfiguration>> GetAllAsync()
        {
            IEnumerable<ProductConfiguration> data = await _repository.GetAllAsync();

            List<ProductConfiguration> list = new List<ProductConfiguration>();

            foreach (ProductConfiguration item in data)
            {
                list.Add(item);
            }

            return list;
        }

        public async Task<(bool Success, string Message)> SaveStructureAsync(ProductConfigurationSaveRequest request)
        {
            if (request == null)
            {
                return (false, "Invalid request.");
            }

            if (request.ProductId <= 0)
            {
                return (false, "Please select product.");
            }

            if (request.Nodes == null || request.Nodes.Count == 0)
            {
                return (false, "Please add at least one node.");
            }

            await _repository.DeleteByProductAsync(request.ProductId);

            int sortOrder = 1;

            foreach (ProductConfigurationNodeRequest node in request.Nodes)
            {
                await SaveNodeRecursive(request.ProductId, null, node, sortOrder);
                sortOrder++;
            }

            return (true, "Product configuration saved successfully.");
        }

        private async Task SaveNodeRecursive(
            int productId,
            int? parentNodeId,
            ProductConfigurationNodeRequest requestNode,
            int sortOrder)
        {
            if (requestNode == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(requestNode.NodeName))
            {
                return;
            }

            ProductConfiguration model = new ProductConfiguration();
            model.ProductId = productId;
            model.ParentNodeId = parentNodeId;
            model.NodeName = requestNode.NodeName.Trim();
            model.InputType = requestNode.InputType;
            model.SortOrder = sortOrder;
            model.IsActive = true;

            int newNodeId = await _repository.AddAsync(model);

            if (requestNode.Children != null && requestNode.Children.Count > 0)
            {
                int childSort = 1;

                foreach (ProductConfigurationNodeRequest child in requestNode.Children)
                {
                    await SaveNodeRecursive(productId, newNodeId, child, childSort);
                    childSort++;
                }
            }
        }
    }
}