using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;

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
            IEnumerable<SMCConfig> data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Create(bool modal = false)
        {
            ViewBag.IsModal = modal;

            IEnumerable<ProductSetup> products = await _productRepo.GetAllAsync();

            int selectedProductId = 0;
            int selectedSMCProductId = 0;
            int selectedSMCProductItemId = 0;

            foreach (ProductSetup product in products)
            {
                selectedProductId = product.ProductId;
                break;
            }

            List<SMCProduct> smcProducts = new List<SMCProduct>();
            if (selectedProductId > 0)
            {
                IEnumerable<SMCProduct> smcProductData = await _smcProductRepo.GetByProductIdAsync(selectedProductId);
                foreach (SMCProduct item in smcProductData)
                {
                    smcProducts.Add(item);
                }

                foreach (SMCProduct item in smcProducts)
                {
                    selectedSMCProductId = item.SMCProductId;
                    break;
                }
            }

            List<SMCProductItem> smcProductItems = new List<SMCProductItem>();
            if (selectedSMCProductId > 0)
            {
                IEnumerable<SMCProductItem> itemData = await _smcProductItemRepo.GetByProductAsync(selectedSMCProductId);
                foreach (SMCProductItem item in itemData)
                {
                    smcProductItems.Add(item);
                }

                foreach (SMCProductItem item in smcProductItems)
                {
                    selectedSMCProductItemId = item.SMCProductItemId;
                    break;
                }
            }

            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
            ViewBag.SMCProductList = new SelectList(smcProducts, "SMCProductId", "SMCProductName", selectedSMCProductId);
            ViewBag.SMCProductItemList = new SelectList(smcProductItems, "SMCProductItemId", "ItemName", selectedSMCProductItemId);

            List<SelectListItem> modes = new List<SelectListItem>();
            modes.Add(new SelectListItem { Value = "Value", Text = "Value" });
            modes.Add(new SelectListItem { Value = "Checkbox", Text = "Checkbox" });

            ViewBag.EntryModeList = new SelectList(modes, "Value", "Text", "Value");

            SMCConfig model = new SMCConfig();
            model.ProductId = selectedProductId;
            model.SMCProductId = selectedSMCProductId;
            model.SMCProductItemId = selectedSMCProductItemId;
            model.IsActive = true;
            model.EntryMode = "Value";
            model.IsChecked = false;
            model.EntryDate = DateTime.Today;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SMCConfig model, bool isModal = false)
        {
            ViewBag.IsModal = isModal;
            model.EntryDate = DateTime.Now.Date;

            if (!ModelState.IsValid)
            {
                await LoadProducts(model.ProductId);
                await LoadSMCProducts(model.ProductId, model.SMCProductId);
                await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
                await LoadEntryModes(model.EntryMode);
                return View(model);
            }

            string? currentUser = User.FindFirst(ClaimTypes.Name)?.Value;
            string result = await _service.AddAsync(model, currentUser);

            string[] parts = result.Split('|', 2);
            string status = parts[0];
            string message = parts.Length > 1 ? parts[1] : "Operation failed.";

            if (status == "success")
            {
                TempData["Success"] = message;
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = message;
            await LoadProducts(model.ProductId);
            await LoadSMCProducts(model.ProductId, model.SMCProductId);
            await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
            await LoadEntryModes(model.EntryMode);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            SMCConfig? data = await _service.GetByIdAsync(id);

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
            if (!ModelState.IsValid)
            {
                await LoadProducts(model.ProductId);
                await LoadSMCProducts(model.ProductId, model.SMCProductId);
                await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
                await LoadEntryModes(model.EntryMode);
                return View(model);
            }

            string? currentUser = User.FindFirst(ClaimTypes.Name)?.Value;
            string result = await _service.UpdateAsync(model, currentUser);

            string[] parts = result.Split('|', 2);
            string status = parts[0];
            string message = parts.Length > 1 ? parts[1] : "Operation failed.";

            if (status == "success")
            {
                TempData["Success"] = message;
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = message;
            await LoadProducts(model.ProductId);
            await LoadSMCProducts(model.ProductId, model.SMCProductId);
            await LoadSMCProductItems(model.SMCProductId, model.SMCProductItemId);
            await LoadEntryModes(model.EntryMode);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            SMCConfig? data = await _service.GetByIdAsync(id);

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
            string? currentUser = User.FindFirst(ClaimTypes.Name)?.Value;
            string result = await _service.DeleteAsync(id, currentUser);

            string[] parts = result.Split('|', 2);
            string status = parts[0];
            string message = parts.Length > 1 ? parts[1] : "Operation failed.";

            if (status == "success")
            {
                TempData["Success"] = message;
            }
            else
            {
                TempData["Error"] = message;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetSMCProductsByProductId(int productId)
        {
            IEnumerable<SMCProduct> data = await _smcProductRepo.GetByProductIdAsync(productId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetSMCProductItemsBySMCProductId(int smcProductId)
        {
            IEnumerable<SMCProductItem> data = await _smcProductItemRepo.GetByProductAsync(smcProductId);
            return Json(data);
        }

        private async Task LoadProducts(int? selectedProductId = null)
        {
            IEnumerable<ProductSetup> products = await _productRepo.GetAllAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
        }

        private async Task LoadSMCProducts(int productId, int? selectedSMCProductId = null)
        {
            IEnumerable<SMCProduct> data = await _smcProductRepo.GetByProductIdAsync(productId);
            ViewBag.SMCProductList = new SelectList(data, "SMCProductId", "SMCProductName", selectedSMCProductId);
        }

        private async Task LoadSMCProductItems(int smcProductId, int? selectedSMCProductItemId = null)
        {
            IEnumerable<SMCProductItem> data = await _smcProductItemRepo.GetByProductAsync(smcProductId);
            ViewBag.SMCProductItemList = new SelectList(data, "SMCProductItemId", "ItemName", selectedSMCProductItemId);
        }

        private Task LoadEntryModes(string selectedEntryMode = null)
        {
            List<SelectListItem> modes = new List<SelectListItem>();
            modes.Add(new SelectListItem { Value = "Value", Text = "Value" });
            modes.Add(new SelectListItem { Value = "Checkbox", Text = "Checkbox" });

            ViewBag.EntryModeList = new SelectList(modes, "Value", "Text", selectedEntryMode);
            return Task.CompletedTask;
        }
    }
}