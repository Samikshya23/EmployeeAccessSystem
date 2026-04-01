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
            await LoadEntryModes();

            ViewBag.SMCProductList = new SelectList(new List<SMCProduct>(), "SMCProductId", "SMCProductName");
            ViewBag.SMCProductItemList = new SelectList(new List<SMCProductItem>(), "SMCProductItemId", "ItemName");

            var model = new SMCConfig
            {
                IsActive = true,
                EntryMode = "Value"
            };

            return View(model);
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
                    await LoadEntryModes();
                    return View(model);
                }

                var result = await _service.AddAsync(model);

                if (result <= 0)
                {
                    ViewBag.Error = "Data could not be saved.";
                    await LoadProducts();
                    await LoadSMCProducts(model.ProductId);
                    await LoadSMCProductItems(model.SMCProductId);
                    await LoadEntryModes();
                    return View(model);
                }

                TempData["Success"] = "Saved successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                await LoadProducts();
                await LoadSMCProducts(model.ProductId);
                await LoadSMCProductItems(model.SMCProductId);
                await LoadEntryModes();
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            await LoadProducts();
            await LoadSMCProducts(data.ProductId);
            await LoadSMCProductItems(data.SMCProductId);
            await LoadEntryModes();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SMCConfig model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await LoadProducts();
                    await LoadSMCProducts(model.ProductId);
                    await LoadSMCProductItems(model.SMCProductId);
                    await LoadEntryModes();
                    return View(model);
                }

                var result = await _service.UpdateAsync(model);

                if (result <= 0)
                {
                    ViewBag.Error = "Data could not be updated.";
                    await LoadProducts();
                    await LoadSMCProducts(model.ProductId);
                    await LoadSMCProductItems(model.SMCProductId);
                    await LoadEntryModes();
                    return View(model);
                }

                TempData["Success"] = "Updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                await LoadProducts();
                await LoadSMCProducts(model.ProductId);
                await LoadSMCProductItems(model.SMCProductId);
                await LoadEntryModes();
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, SMCConfig model)
        {
            var result = await _service.DeleteAsync(id);

            if (result > 0)
            {
                TempData["Success"] = "Deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Delete failed.";
            }

            return RedirectToAction(nameof(Index));
        }

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

        private Task LoadEntryModes()
        {
            var modes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Value", Text = "Value" },
                new SelectListItem { Value = "Checkbox", Text = "Checkbox" }
            };

            ViewBag.EntryModeList = new SelectList(modes, "Value", "Text");
            return Task.CompletedTask;
        }
    }
}