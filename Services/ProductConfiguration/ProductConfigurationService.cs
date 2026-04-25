using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<ProductConfigurationIndexItem>> GetIndexAsync()
        {
            IEnumerable<ProductConfiguration> data = await _repository.GetAllAsync();

            List<ProductConfiguration> flatList = new List<ProductConfiguration>();

            foreach (ProductConfiguration item in data)
            {
                flatList.Add(item);
            }

            List<ProductConfigurationIndexItem> result = new List<ProductConfigurationIndexItem>();

            var productGroups = flatList
                .GroupBy(x => new { x.ProductId, x.ProductName })
                .OrderBy(x => x.Key.ProductName);

            foreach (var group in productGroups)
            {
                ProductConfigurationIndexItem indexItem = new ProductConfigurationIndexItem();
                indexItem.ProductId = group.Key.ProductId;
                indexItem.ProductName = group.Key.ProductName;
                indexItem.Nodes = BuildTree(group.ToList());

                result.Add(indexItem);
            }

            return result;
        }

        public async Task<List<ProductConfiguration>> GetTreeByProductIdAsync(int productId)
        {
            IEnumerable<ProductConfiguration> data = await _repository.GetByProductIdAsync(productId);

            List<ProductConfiguration> flatList = new List<ProductConfiguration>();

            foreach (ProductConfiguration item in data)
            {
                flatList.Add(item);
            }

            return BuildTree(flatList);
        }

        public async Task<(bool Success, string Message)> SaveStructureAsync(ProductConfigurationSaveRequest request, string createdBy)
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

            await _repository.DeleteByProductAsync(request.ProductId, createdBy);

            int sortOrder = 1;

            foreach (ProductConfigurationNodeRequest node in request.Nodes)
            {
                await SaveNodeRecursive(request.ProductId, null, node, sortOrder, createdBy);
                sortOrder++;
            }

            return (true, "Product configuration saved successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteByProductAsync(int productId, string deletedBy)
        {
            if (productId <= 0)
            {
                return (false, "Invalid product configuration.");
            }

            await _repository.DeleteByProductAsync(productId, deletedBy);

            return (true, "Product configuration deleted successfully.");
        }

        private async Task SaveNodeRecursive(
            int productId,
            int? parentNodeId,
            ProductConfigurationNodeRequest requestNode,
            int sortOrder,
            string createdBy)
        {
            if (requestNode == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(requestNode.NodeName))
            {
                return;
            }

            string nodeType = requestNode.NodeType;

            if (string.IsNullOrWhiteSpace(nodeType))
            {
                nodeType = "Block";
            }

            ProductConfiguration model = new ProductConfiguration();
            model.ProductId = productId;
            model.ParentNodeId = parentNodeId;
            model.NodeName = requestNode.NodeName.Trim();
            model.NodeType = nodeType;
            model.InputType = requestNode.InputType;
            model.SortOrder = sortOrder;
            model.IsActive = true;
            model.CreatedBy = createdBy;

            int newNodeId = await _repository.AddAsync(model);

            if (requestNode.Children != null && requestNode.Children.Count > 0)
            {
                int childSort = 1;

                foreach (ProductConfigurationNodeRequest child in requestNode.Children)
                {
                    await SaveNodeRecursive(productId, newNodeId, child, childSort, createdBy);
                    childSort++;
                }
            }
        }

        private List<ProductConfiguration> BuildTree(List<ProductConfiguration> flatList)
        {
            List<ProductConfiguration> roots = new List<ProductConfiguration>();

            foreach (ProductConfiguration item in flatList)
            {
                item.Children = new List<ProductConfiguration>();
            }

            foreach (ProductConfiguration item in flatList)
            {
                if (item.ParentNodeId == null)
                {
                    roots.Add(item);
                }
                else
                {
                    ProductConfiguration parent = flatList.FirstOrDefault(x => x.NodeId == item.ParentNodeId.Value);

                    if (parent != null)
                    {
                        parent.Children.Add(item);
                    }
                }
            }

            return roots.OrderBy(x => x.SortOrder).ToList();
        }
    }
}