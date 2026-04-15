using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Security.Claims;

namespace EmployeeAccessSystem.Controllers
{
    public class PingConfigController : Controller
    {
        private readonly IPingConfigService _service;
        private readonly IProductSetupRepositories _productRepo;
        private readonly IPingProductRepository _pingProductRepo;
        private readonly IPingProductItemRepository _pingProductItemRepo;

        public PingConfigController(
            IPingConfigService service,
            IProductSetupRepositories productRepo,
            IPingProductRepository pingProductRepo,
            IPingProductItemRepository pingProductItemRepo)
        {
            _service = service;
            _productRepo = productRepo;
            _pingProductRepo = pingProductRepo;
            _pingProductItemRepo = pingProductItemRepo;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<PingConfig> data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Create(bool modal = false)
        {
            ViewBag.IsModal = modal;

            IEnumerable<ProductSetup> products = await _productRepo.GetAllAsync();

            int selectedProductId = 0;
            int selectedPingProductId = 0;
            int selectedPingProductItemId = 0;

            foreach (ProductSetup product in products)
            {
                selectedProductId = product.ProductId;
                break;
            }

            List<PingProduct> pingProducts = new List<PingProduct>();
            if (selectedProductId > 0)
            {
                IEnumerable<PingProduct> pingProductData = await _pingProductRepo.GetByProductIdAsync(selectedProductId);
                foreach (PingProduct item in pingProductData)
                {
                    pingProducts.Add(item);
                }

                foreach (PingProduct item in pingProducts)
                {
                    selectedPingProductId = item.PingProductId;
                    break;
                }
            }

            List<PingProductItem> pingProductItems = new List<PingProductItem>();
            if (selectedPingProductId > 0)
            {
                IEnumerable<PingProductItem> itemData = await _pingProductItemRepo.GetByPingProductIdAsync(selectedPingProductId);
                foreach (PingProductItem item in itemData)
                {
                    pingProductItems.Add(item);
                }

                foreach (PingProductItem item in pingProductItems)
                {
                    selectedPingProductItemId = item.PingProductItemId;
                    break;
                }
            }

            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
            ViewBag.PingProductList = new SelectList(pingProducts, "PingProductId", "PingProductName", selectedPingProductId);
            ViewBag.PingProductItemList = new SelectList(pingProductItems, "PingProductItemId", "ItemName", selectedPingProductItemId);

            List<SelectListItem> modes = new List<SelectListItem>();
            modes.Add(new SelectListItem { Value = "Value", Text = "Value" });
            modes.Add(new SelectListItem { Value = "Checkbox", Text = "Checkbox" });

            ViewBag.EntryModeList = new SelectList(modes, "Value", "Text", "Value");

            PingConfig model = new PingConfig();
            model.ProductId = selectedProductId;
            model.PingProductId = selectedPingProductId;
            model.PingProductItemId = selectedPingProductItemId;
            model.IsActive = true;
            model.EntryMode = "Value";
            model.IsChecked = false;
            model.EntryDate = DateTime.Today;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PingConfig model, bool isModal = false)
        {
            ViewBag.IsModal = isModal;
            model.EntryDate = DateTime.Now.Date;

            if (!ModelState.IsValid)
            {
                await LoadProducts(model.ProductId);
                await LoadPingProducts(model.ProductId, model.PingProductId);
                await LoadPingProductItems(model.PingProductId, model.PingProductItemId);
                await LoadEntryModes(model.EntryMode);
                return View(model);
            }

            string? currentUser = User.FindFirst(ClaimTypes.Email)?.Value
                                  ?? User.FindFirst("email")?.Value
                                  ?? User.Identity?.Name;

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
            await LoadPingProducts(model.ProductId, model.PingProductId);
            await LoadPingProductItems(model.PingProductId, model.PingProductItemId);
            await LoadEntryModes(model.EntryMode);
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            PingConfig? data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            await LoadProducts(data.ProductId);
            await LoadPingProducts(data.ProductId, data.PingProductId);
            await LoadPingProductItems(data.PingProductId, data.PingProductItemId);
            await LoadEntryModes(data.EntryMode);

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PingConfig model)
        {
            if (!ModelState.IsValid)
            {
                await LoadProducts(model.ProductId);
                await LoadPingProducts(model.ProductId, model.PingProductId);
                await LoadPingProductItems(model.PingProductId, model.PingProductItemId);
                await LoadEntryModes(model.EntryMode);
                return View(model);
            }

            string? currentUser = User.FindFirst(ClaimTypes.Email)?.Value
                                  ?? User.FindFirst("email")?.Value
                                  ?? User.Identity?.Name;

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
            await LoadPingProducts(model.ProductId, model.PingProductId);
            await LoadPingProductItems(model.PingProductId, model.PingProductItemId);
            await LoadEntryModes(model.EntryMode);
            return View(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            PingConfig? data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, PingConfig model)
        {
            string? currentUser = User.FindFirst(ClaimTypes.Email)?.Value
                                  ?? User.FindFirst("email")?.Value
                                  ?? User.Identity?.Name;

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
        public async Task<IActionResult> GetPingProductsByProductId(int productId)
        {
            IEnumerable<PingProduct> data = await _pingProductRepo.GetByProductIdAsync(productId);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetPingProductItemsByPingProductId(int pingProductId)
        {
            IEnumerable<PingProductItem> data = await _pingProductItemRepo.GetByPingProductIdAsync(pingProductId);
            return Json(data);
        }

        private async Task LoadProducts(int? selectedProductId = null)
        {
            IEnumerable<ProductSetup> products = await _productRepo.GetAllAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName", selectedProductId);
        }

        private async Task LoadPingProducts(int productId, int? selectedPingProductId = null)
        {
            IEnumerable<PingProduct> data = await _pingProductRepo.GetByProductIdAsync(productId);
            ViewBag.PingProductList = new SelectList(data, "PingProductId", "PingProductName", selectedPingProductId);
        }

        private async Task LoadPingProductItems(int pingProductId, int? selectedPingProductItemId = null)
        {
            IEnumerable<PingProductItem> data = await _pingProductItemRepo.GetByPingProductIdAsync(pingProductId);
            ViewBag.PingProductItemList = new SelectList(data, "PingProductItemId", "ItemName", selectedPingProductItemId);
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