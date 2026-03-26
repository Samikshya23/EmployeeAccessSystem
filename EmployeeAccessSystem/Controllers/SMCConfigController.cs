using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class SMCConfigController : Controller
    {
        private readonly ISMCConfigService _service;
        private readonly IProductSetupRepositories _productRepo;
        private readonly ISMCProductRepository _smcProductRepo;
        private readonly ISMCProductItemRepositories _smcProductItemRepo;

        public SMCConfigController(
            ISMCConfigService service,
            IProductSetupRepositories productRepo,
            ISMCProductRepository smcProductRepo,
            ISMCProductItemRepositories smcProductItemRepo)
        {
            _service = service;
            _productRepo = productRepo;
            _smcProductRepo = smcProductRepo;
            _smcProductItemRepo = smcProductItemRepo;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            await LoadProducts();

            ViewBag.SMCProductList = new SelectList(
                new List<SMCProduct>(),
                "SMCProductId",
                "SMCProductName"
            );

            ViewBag.SMCProductItemList = new SelectList(
                new List<SMCProductItem>(),
                "SMCProductItemId",
                "ItemName"
            );

            SMCConfig model = new SMCConfig();
            model.IsActive = true;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SMCConfig model)
        {
            model.EntryDate = DateTime.Now;

            if (!ModelState.IsValid)
            {
                await LoadProducts();
                await LoadSMCProducts(model.ProductId);
                await LoadSMCProductItems(model.SMCProductId);
                return View(model);
            }

            var result = await _service.AddAsync(model);

            if (result <= 0)
            {
                ViewBag.Error = "Failed to save SMC Config.";
                await LoadProducts();
                await LoadSMCProducts(model.ProductId);
                await LoadSMCProductItems(model.SMCProductId);
                return View(model);
            }

            TempData["Success"] = "SMC Config saved successfully.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> GetSMCProductsByProductId(int productId)
        {
            var data = await _smcProductRepo.GetAllAsync();
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSMCProductItemsBySMCProductId(int smcProductId)
        {
            var data = await _smcProductItemRepo.GetByProductAsync(smcProductId);
            return Json(data);
        }

        private async Task LoadProducts()
        {
            var products = await _productRepo.GetAllAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");
        }

        private async Task LoadSMCProducts(int productId)
        {
            var data = await _smcProductRepo.GetAllAsync();

            ViewBag.SMCProductList = new SelectList(
                data,
                "SMCProductId",
                "SMCProductName"
            );
        }

        private async Task LoadSMCProductItems(int smcProductId)
        {
            var data = await _smcProductItemRepo.GetByProductAsync(smcProductId);

            ViewBag.SMCProductItemList = new SelectList(
                data,
                "SMCProductItemId",
                "ItemName"
            );
        }
    }
}