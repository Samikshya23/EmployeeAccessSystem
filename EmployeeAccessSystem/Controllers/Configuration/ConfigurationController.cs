using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationService _service;
        private readonly IProductSetupRepositories _productRepo;

        public ConfigurationController(
            IConfigurationService service,
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

            if (productId.HasValue && productId.Value > 0)
            {
                var data = await _service.GetByProductIdAsync(productId.Value);
                return View(data);
            }

            return View(new List<EmployeeAccessSystem.Models.Configuration>());
        }

        private async Task LoadProducts(int? selectedProductId = null)
        {
            var products = await _productRepo.GetActiveAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
        }
    }
}