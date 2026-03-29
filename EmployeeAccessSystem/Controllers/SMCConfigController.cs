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

        // ===================== LIST =====================
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        // ===================== CREATE =====================
        public async Task<IActionResult> Create()
        {
            await LoadProducts();

            ViewBag.SMCProductList = new SelectList(new List<SMCProduct>(), "SMCProductId", "SMCProductName");
            ViewBag.SMCProductItemList = new SelectList(new List<SMCProductItem>(), "SMCProductItemId", "ItemName");

            return View(new SMCConfig { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SMCConfig model)
        {
            try
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
                    ViewBag.Error = "Insert failed.";
                    return View(model);
                }

                TempData["Success"] = "Saved successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // ===================== EDIT =====================
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return NotFound();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SMCConfig model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var result = await _service.UpdateAsync(model);

                if (result <= 0)
                {
                    ViewBag.Error = "Update failed.";
                    return View(model);
                }

                TempData["Success"] = "Updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // ===================== DELETE =====================
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            if (result > 0)
                TempData["Success"] = "Deleted successfully.";
            else
                TempData["Error"] = "Delete failed.";

            return RedirectToAction(nameof(Index));
        }

        // ===================== DROPDOWNS =====================
        [HttpGet]
        public async Task<IActionResult> GetSMCProductsByProductId(int productId)
        {
            var data = await _smcProductRepo.GetByProductIdAsync(productId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSMCProductItemsBySMCProductId(int smcProductId)
        {
            var data = await _smcProductItemRepo.GetByProductAsync(smcProductId);
            return Json(data);
        }

        // ===================== HELPERS =====================
        private async Task LoadProducts()
        {
            var products = await _productRepo.GetAllAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");
        }

        private async Task LoadSMCProducts(int productId)
        {
            var data = await _smcProductRepo.GetByProductIdAsync(productId);
            ViewBag.SMCProductList = new SelectList(data, "SMCProductId", "SMCProductName");
        }

        private async Task LoadSMCProductItems(int smcProductId)
        {
            var data = await _smcProductItemRepo.GetByProductAsync(smcProductId);
            ViewBag.SMCProductItemList = new SelectList(data, "SMCProductItemId", "ItemName");
        }
    }
}