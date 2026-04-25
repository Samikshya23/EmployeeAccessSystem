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

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetIndexAsync();
            return View(data);
        }

        public async Task<IActionResult> Add(int? productId)
        {
            var products = await _productRepo.GetActiveAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", productId);

            ViewBag.SelectedProductId = productId.HasValue ? productId.Value : 0;

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
            string userName = "System";

            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            var result = await _service.SaveStructureAsync(request, userName);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            string userName = "System";

            if (User != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                userName = User.Identity.Name;
            }

            var result = await _service.DeleteByProductAsync(productId, userName);

            TempData[result.Success ? "Success" : "Error"] = result.Message;

            return RedirectToAction("Index");
        }
    }
}