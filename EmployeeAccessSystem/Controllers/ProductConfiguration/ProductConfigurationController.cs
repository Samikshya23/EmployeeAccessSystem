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
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Add()
        {
            var products = await _productRepo.GetActiveAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");

            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> SaveStructure([FromBody] ProductConfigurationSaveRequest request)
        {
            var result = await _service.SaveStructureAsync(request);

            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }
    }
}