using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class ProductConfigurationController : Controller
    {
        private readonly IProductConfigurationService _service;
        private readonly IProductSetupRepositories _productRepo;

        public ProductConfigurationController(
            IProductConfigurationService service,
            IProductSetupRepositories productRepo)
        {
            _service = service;
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index(int? selectedProductId)
        {
            var products = await _productRepo.GetActiveAsync();

            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);

            if (selectedProductId.HasValue)
            {
                ViewBag.SelectedProductId = selectedProductId.Value;
            }
            else
            {
                ViewBag.SelectedProductId = 0;
            }

            if (!selectedProductId.HasValue || selectedProductId.Value <= 0)
            {
                return View(new List<ProductConfigurationIndexItem>());
            }

            var data = await _service.GetIndexAsync();

            List<ProductConfigurationIndexItem> result = new List<ProductConfigurationIndexItem>();

            foreach (ProductConfigurationIndexItem item in data)
            {
                if (item.ProductId == selectedProductId.Value)
                {
                    result.Add(item);
                }
            }

            return View(result);
        }

        public async Task<IActionResult> Add(int? productId)
        {
            var products = await _productRepo.GetActiveAsync();

            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", productId);

            if (productId.HasValue)
            {
                ViewBag.SelectedProductId = productId.Value;
            }
            else
            {
                ViewBag.SelectedProductId = 0;
            }

            if (productId.HasValue && productId.Value > 0)
            {
                var nodes = await _service.GetTreeByProductIdAsync(productId.Value);

                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                ViewBag.ExistingJson = JsonSerializer.Serialize(nodes, options);
            }
            else
            {
                ViewBag.ExistingJson = "[]";
            }

            return PartialView("_Add");
        }

        [HttpPost]
        public async Task<IActionResult> SaveStructure([FromBody] ProductConfigurationSaveRequest request)
        {
            try
            {
                string userName = "System";

                if (User != null)
                {
                    if (User.Identity != null)
                    {
                        if (User.Identity.IsAuthenticated)
                        {
                            userName = User.Identity.Name;
                        }
                    }
                }

                var result = await _service.SaveStructureAsync(request, userName);

                return Json(new
                {
                    success = result.Success,
                    message = result.Message,
                    productId = request.ProductId
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        public async Task<IActionResult> DeleteNode(int nodeId)
        {
            ProductConfiguration node = await _service.GetNodeByIdAsync(nodeId);

            if (node == null)
            {
                node = new ProductConfiguration();
            }

            return PartialView("_DeleteNode", node);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteNodeConfirmed(int nodeId, int productId)
        {
            string userName = "System";

            if (User != null)
            {
                if (User.Identity != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        userName = User.Identity.Name;
                    }
                }
            }

            var result = await _service.DeleteNodeAsync(nodeId, userName);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("Index", new { selectedProductId = productId });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            string userName = "System";

            if (User != null)
            {
                if (User.Identity != null)
                {
                    if (User.Identity.IsAuthenticated)
                    {
                        userName = User.Identity.Name;
                    }
                }
            }

            var result = await _service.DeleteByProductAsync(productId, userName);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
            }
            else
            {
                TempData["Error"] = result.Message;
            }

            return RedirectToAction("Index");
        }
    }
}