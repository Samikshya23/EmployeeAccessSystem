using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using EmployeeAccessSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeAccessSystem.Controllers
{
    public class PingConfigController : Controller
    {
        private readonly IPingConfigService _service;
        private readonly IPingProductRepository _pingProductRepository;
        private readonly IProductSetupRepositories _productRepo;

        public PingConfigController(
            IPingConfigService service,
            IPingProductRepository pingProductRepository,
            IProductSetupRepositories productRepo)
        {
            _service = service;
            _pingProductRepository = pingProductRepository;
            _productRepo = productRepo;
        }

        public async Task<IActionResult> Index(string successMessage, string errorMessage)
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                TempData["Success"] = successMessage;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                TempData["Error"] = errorMessage;
            }

            var data = await _service.GetAllAsync();
            return View(data);
        }

        public async Task<IActionResult> Create()
        {
            await LoadProducts();

            string[] emptyIPs = new string[0];
            ViewBag.IPList = new SelectList(emptyIPs);

            PingConfig model = new PingConfig();
            model.IsActive = true;
            model.EntryMode = "Checkbox";
            model.IsChecked = true;

            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PingConfig model)
        {
            string currentUser = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name ?? "System";
            model.EntryDate = DateTime.Now;

            if (model.ProductId <= 0)
            {
                ModelState.AddModelError("ProductId", "Please select a product.");
            }

            if (string.IsNullOrWhiteSpace(model.IPAddress))
            {
                ModelState.AddModelError("IPAddress", "Please select IP address.");
            }

            if (model.PingProductId <= 0)
            {
                ModelState.AddModelError("PingProductId", "Please select server host name.");
            }

            if (string.IsNullOrWhiteSpace(model.EntryMode))
            {
                ModelState.AddModelError("EntryMode", "Please select save option.");
            }

            if (model.EntryMode == "Value" && string.IsNullOrWhiteSpace(model.ConfigValue))
            {
                ModelState.AddModelError("ConfigValue", "Please enter value.");
            }

            if (!ModelState.IsValid)
            {
                await LoadProducts();
                await LoadDistinctIPs(model.ProductId);
                await LoadHosts(model.ProductId, model.IPAddress);
                return PartialView(model);
            }

            string message = await _service.AddAsync(model, currentUser);

            if (message == "Ping Config added successfully.")
            {
                return Json(new { success = true, message = message });
            }

            ViewBag.Error = message;
            await LoadProducts();
            await LoadDistinctIPs(model.ProductId);
            await LoadHosts(model.ProductId, model.IPAddress);

            return PartialView(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            PingConfig data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            await LoadProducts();
            await LoadDistinctIPs(data.ProductId);
            await LoadHosts(data.ProductId, data.IPAddress);

            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PingConfig model)
        {
            string currentUser = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name ?? "System";

            if (model.ProductId <= 0)
            {
                ModelState.AddModelError("ProductId", "Please select a product.");
            }

            if (string.IsNullOrWhiteSpace(model.IPAddress))
            {
                ModelState.AddModelError("IPAddress", "Please select IP address.");
            }

            if (model.PingProductId <= 0)
            {
                ModelState.AddModelError("PingProductId", "Please select server host name.");
            }

            if (string.IsNullOrWhiteSpace(model.EntryMode))
            {
                ModelState.AddModelError("EntryMode", "Please select save option.");
            }

            if (model.EntryMode == "Value" && string.IsNullOrWhiteSpace(model.ConfigValue))
            {
                ModelState.AddModelError("ConfigValue", "Please enter value.");
            }

            PingConfig existingData = await _service.GetByIdAsync(model.PingConfigId);
            if (existingData != null)
            {
                model.EntryDate = existingData.EntryDate;
            }

            if (!ModelState.IsValid)
            {
                await LoadProducts();
                await LoadDistinctIPs(model.ProductId);
                await LoadHosts(model.ProductId, model.IPAddress);
                return PartialView(model);
            }

            string message = await _service.UpdateAsync(model, currentUser);

            if (message == "Ping Config updated successfully.")
            {
                return Json(new { success = true, message = message });
            }

            ViewBag.Error = message;
            await LoadProducts();
            await LoadDistinctIPs(model.ProductId);
            await LoadHosts(model.ProductId, model.IPAddress);

            return PartialView(model);
        }

        public async Task<IActionResult> Delete(int id)
        {
            PingConfig data = await _service.GetByIdAsync(id);

            if (data == null)
            {
                return NotFound();
            }

            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int pingConfigId)
        {
            string currentUser = User.FindFirstValue(ClaimTypes.Email) ?? User.Identity?.Name ?? "System";

            string message = await _service.DeleteAsync(pingConfigId, currentUser);

            if (message == "Ping Config deleted successfully.")
            {
                return Json(new { success = true, message = message });
            }

            PingConfig data = await _service.GetByIdAsync(pingConfigId);
            if (data == null)
            {
                data = new PingConfig();
                data.PingConfigId = pingConfigId;
            }

            ViewBag.Error = message;
            return PartialView("Delete", data);
        }

        [HttpGet]
        public async Task<JsonResult> GetIPsByProduct(int productId)
        {
            var data = await _pingProductRepository.GetByProductIdAsync(productId);

            List<string> result = new List<string>();
            HashSet<string> seenIPs = new HashSet<string>();

            foreach (var item in data)
            {
                if (item.IsActive && !string.IsNullOrWhiteSpace(item.IPAddress))
                {
                    if (!seenIPs.Contains(item.IPAddress))
                    {
                        seenIPs.Add(item.IPAddress);
                        result.Add(item.IPAddress);
                    }
                }
            }

            result.Sort();

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetHostsByProductAndIP(int productId, string ipAddress)
        {
            var data = await _pingProductRepository.GetByProductIdAsync(productId);

            List<object> result = new List<object>();

            foreach (var item in data)
            {
                if (item.IsActive && item.IPAddress == ipAddress)
                {
                    result.Add(new
                    {
                        pingProductId = item.PingProductId,
                        serverHostName = item.ServerHostName
                    });
                }
            }

            return Json(result);
        }

        private async Task LoadProducts()
        {
            var products = await _productRepo.GetActiveAsync();
            ViewBag.ProductList = new SelectList(products, "ProductId", "ProductName");
        }

        private async Task LoadDistinctIPs(int productId)
        {
            if (productId <= 0)
            {
                ViewBag.IPList = new SelectList(new List<string>());
                return;
            }

            var data = await _pingProductRepository.GetByProductIdAsync(productId);

            List<string> ipList = new List<string>();
            HashSet<string> seenIPs = new HashSet<string>();

            foreach (var item in data)
            {
                if (item.IsActive && !string.IsNullOrWhiteSpace(item.IPAddress))
                {
                    if (!seenIPs.Contains(item.IPAddress))
                    {
                        seenIPs.Add(item.IPAddress);
                        ipList.Add(item.IPAddress);
                    }
                }
            }

            ipList.Sort();

            ViewBag.IPList = new SelectList(ipList);
        }

        private async Task LoadHosts(int productId, string ipAddress)
        {
            if (productId <= 0 || string.IsNullOrWhiteSpace(ipAddress))
            {
                ViewBag.HostList = new SelectList(new List<PingProduct>(), "PingProductId", "ServerHostName");
                return;
            }

            var data = await _pingProductRepository.GetByProductIdAsync(productId);

            List<PingProduct> hostList = new List<PingProduct>();

            foreach (var item in data)
            {
                if (item.IsActive && item.IPAddress == ipAddress)
                {
                    hostList.Add(item);
                }
            }

            hostList.Sort(delegate (PingProduct x, PingProduct y)
            {
                string name1 = x.ServerHostName ?? "";
                string name2 = y.ServerHostName ?? "";
                return string.Compare(name1, name2, StringComparison.OrdinalIgnoreCase);
            });

            ViewBag.HostList = new SelectList(hostList, "PingProductId", "ServerHostName");
        }
    }
}