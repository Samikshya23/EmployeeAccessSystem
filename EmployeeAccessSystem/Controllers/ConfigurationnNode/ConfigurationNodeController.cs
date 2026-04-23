using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class ConfigurationNodeController : Controller
    {
        private readonly IConfigurationNodeService _service;
        private readonly IProductSetupRepositories _productRepo;
        public ConfigurationNodeController(
            IConfigurationNodeService service,
            IProductSetupRepositories productRepo)
        {
            _service = service;
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index(int? productId, string successMessage, string errorMessage)
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                TempData["Success"] = successMessage;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                TempData["Error"] = errorMessage;
            }

            await LoadProducts(productId);

            ViewBag.SelectedProductId = productId;

            if (productId.HasValue && productId.Value > 0)
            {
                List<ConfigurationNode> data = await _service.GetByProductIdAsync(productId.Value);
                return View(data);
            }

            return View(new List<ConfigurationNode>());
        }

        public async Task<IActionResult> Create(int? productId, int? parentNodeId)
        {
            await LoadProducts(productId);
            await LoadParentNodes(productId, null);

            ConfigurationNode model = new ConfigurationNode();
            model.ProductId = productId ?? 0;
            model.ParentNodeId = parentNodeId;
            model.IsActive = true;

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConfigurationNode model)
        {
            await LoadProducts(model.ProductId);
            await LoadParentNodes(model.ProductId, null);

            var result = await _service.AddAsync(model);

            if (result.Success)
            {
                return RedirectToAction("Index", new
                {
                    productId = model.ProductId,
                    successMessage = result.Message
                });
            }

            ViewBag.Error = result.Message;
            return PartialView(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ConfigurationNode model = await _service.GetByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            await LoadProducts(model.ProductId);
            await LoadParentNodes(model.ProductId, model.NodeId);

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ConfigurationNode model)
        {
            await LoadProducts(model.ProductId);
            await LoadParentNodes(model.ProductId, model.NodeId);

            var result = await _service.UpdateAsync(model);

            if (result.Success)
            {
                return RedirectToAction("Index", new
                {
                    productId = model.ProductId,
                    successMessage = result.Message
                });
            }

            ViewBag.Error = result.Message;
            return PartialView(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            ConfigurationNode model = await _service.GetByIdAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int nodeId, int productId)
        {
            var result = await _service.DeleteAsync(nodeId);

            return RedirectToAction("Index", new
            {
                productId = productId,
                successMessage = result.Success ? result.Message : null,
                errorMessage = result.Success ? null : result.Message
            });
        }

        private async Task LoadProducts(int? selectedProductId)
        {
            var products = await _productRepo.GetActiveAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
        }

        private async Task LoadParentNodes(int? productId, int? currentNodeId)
        {
            if (productId.HasValue && productId.Value > 0)
            {
                List<ConfigurationNode> nodes = await _service.GetFlatByProductIdAsync(productId.Value);
                List<ConfigurationNode> parentNodes = new List<ConfigurationNode>();

                foreach (ConfigurationNode item in nodes)
                {
                    if (!currentNodeId.HasValue || item.NodeId != currentNodeId.Value)
                    {
                        parentNodes.Add(item);
                    }
                }

                ViewBag.ParentNodeList = new SelectList(parentNodes, "NodeId", "NodeName");
            }
            else
            {
                ViewBag.ParentNodeList = new SelectList(new List<ConfigurationNode>(), "NodeId", "NodeName");
            }
        }
    }
}