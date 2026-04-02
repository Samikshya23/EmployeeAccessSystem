using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

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

        public async Task<IActionResult> Create(bool modal = false)
        {
            ViewBag.IsModal = modal;

            var products = await _productRepo.GetAllAsync();
            int selectedProductId = 0;
            int selectedSMCProductId = 0;
            int selectedSMCProductItemId = 0;

            if (products != null && products.Any())
            {
                selectedProductId = products.First().ProductId;
            }

            var smcProducts = new List<SMCProduct>();
            if (selectedProductId > 0)
            {
                smcProducts = (await _smcProductRepo.GetByProductIdAsync(selectedProductId)).ToList();
                if (smcProducts.Any())
                {
                    selectedSMCProductId = smcProducts.First().SMCProductId;
                }
            }

            var smcProductItems = new List<SMCProductItem>();
            if (selectedSMCProductId > 0)
            {
                smcProductItems = (await _smcProductItemRepo.GetByProductAsync(selectedSMCProductId)).ToList();
                if (smcProductItems.Any())
                {
                    selectedSMCProductItemId = smcProductItems.First().SMCProductItemId;
                }
            }

            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
            ViewBag.SMCProductList = new SelectList(smcProducts, "SMCProductId", "SMCProductName", selectedSMCProductId);
            ViewBag.SMCProductItemList = new SelectList(smcProductItems, "SMCProductItemId", "ItemName", selectedSMCProductItemId);

            var modes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Value", Text = "Value" },
                new SelectListItem { Value = "Checkbox", Text = "Checkbox" }
            };
            ViewBag.EntryModeList = new SelectList(modes, "Value", "Text", "Value");

            var model = new SMCConfig
            {
                ProductId = selectedProductId,
                SMCProductId = selectedSMCProductId,
                SMCProductItemId = selectedSMCProductItemId,
                IsActive = true,
                EntryMode = "Value",
                IsChecked = false
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SMCConfig model, bool isModal = false)
        {
            try
            {
                model.EntryDate = DateTime.Now;
                ViewBag.IsModal = isModal;

                if (!ModelState.IsValid)
                {
                    await LoadProducts(model.ProductId);
                    await LoadSMCProducts(model.ProductId, model.SMCProductId);
                    await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
                    await LoadEntryModes(model.EntryMode);
                    return View(model);
                }

                var result = await _service.AddAsync(model);

                if (result <= 0)
                {
                    ViewBag.Error = "Data could not be saved.";
                    await LoadProducts(model.ProductId);
                    await LoadSMCProducts(model.ProductId, model.SMCProductId);
                    await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
                    await LoadEntryModes(model.EntryMode);
                    return View(model);
                }

                TempData["Success"] = "Saved successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                ViewBag.IsModal = isModal;
                await LoadProducts(model.ProductId);
                await LoadSMCProducts(model.ProductId, model.SMCProductId);
                await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
                await LoadEntryModes(model.EntryMode);
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

            await LoadProducts(data.ProductId);
            await LoadSMCProducts(data.ProductId, data.SMCProductId);
            await LoadSMCProductItems(data.SMCProductId, data.SMCProductItemId);
            await LoadEntryModes(data.EntryMode);

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
                    await LoadProducts(model.ProductId);
                    await LoadSMCProducts(model.ProductId, model.SMCProductId);
                    await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
                    await LoadEntryModes(model.EntryMode);
                    return View(model);
                }

                var result = await _service.UpdateAsync(model);

                if (result <= 0)
                {
                    ViewBag.Error = "Data could not be updated.";
                    await LoadProducts(model.ProductId);
                    await LoadSMCProducts(model.ProductId, model.SMCProductId);
                    await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
                    await LoadEntryModes(model.EntryMode);
                    return View(model);
                }

                TempData["Success"] = "Updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                await LoadProducts(model.ProductId);
                await LoadSMCProducts(model.ProductId, model.SMCProductId);
                await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
                await LoadEntryModes(model.EntryMode);
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

        private async Task LoadProducts(int? selectedProductId = null)
        {
            var products = await _productRepo.GetAllAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
        }

        private async Task LoadSMCProducts(int productId, int? selectedSMCProductId = null)
        {
            var data = await _smcProductRepo.GetByProductIdAsync(productId);
            ViewBag.SMCProductList = new SelectList(data, "SMCProductId", "SMCProductName", selectedSMCProductId);
        }

        private async Task LoadSMCProductItems(int smcProductId, int? selectedSMCProductItemId = null)
        {
            var data = await _smcProductItemRepo.GetByProductAsync(smcProductId);
            ViewBag.SMCProductItemList = new SelectList(data, "SMCProductItemId", "ItemName", selectedSMCProductItemId);
        }

        private Task LoadEntryModes(string selectedEntryMode = null)
        {
            var modes = new List<SelectListItem>
            {
                new SelectListItem { Value = "Value", Text = "Value" },
                new SelectListItem { Value = "Checkbox", Text = "Checkbox" }
            };

            ViewBag.EntryModeList = new SelectList(modes, "Value", "Text", selectedEntryMode);
            return Task.CompletedTask;
        }
    }
}