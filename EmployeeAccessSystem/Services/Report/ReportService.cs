using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IProductSetupService _productSetupService;

        public ReportService(IReportRepository reportRepository, IProductSetupService productSetupService)
        {
            _reportRepository = reportRepository;
            _productSetupService = productSetupService;
        }

        public async Task<ReportPageViewModel> GetReportPageAsync(int? selectedProductId, DateTime? fromDate, DateTime? toDate)
        {
            ReportPageViewModel model = new ReportPageViewModel();

            IEnumerable<ProductSetup> productData = await _productSetupService.GetAllAsync();
            model.ProductList = new List<ProductSetup>();

            foreach (ProductSetup item in productData)
            {
                model.ProductList.Add(item);
            }

            if (!fromDate.HasValue)
            {
                fromDate = DateTime.Today;
            }

            if (!toDate.HasValue)
            {
                toDate = DateTime.Today;
            }

            model.SelectedProductId = selectedProductId;
            model.FromDate = fromDate;
            model.ToDate = toDate;

            if (!selectedProductId.HasValue)
            {
                model.HasData = false;
                return model;
            }

            int days = (toDate.Value - fromDate.Value).Days;

            if (days < 0)
            {
                model.Message = "From Date cannot be greater than To Date.";
                return model;
            }

            if (days > 31)
            {
                model.Message = "Maximum 31 days allowed.";
                return model;
            }

            string productName = GetProductName(model.ProductList, selectedProductId.Value);
            string flag = "SMSC_REPORT";

            if (IsPingProduct(productName))
            {
                flag = "PING_REPORT";
                model.IsPingReport = true;
                model.IsFortigateReport = false;
            }
            else if (IsFortigateProduct(productName))
            {
                flag = "FORTIGATE_REPORT";
                model.IsPingReport = false;
                model.IsFortigateReport = true;
            }
            else
            {
                flag = "SMSC_REPORT";
                model.IsPingReport = false;
                model.IsFortigateReport = false;
            }

            model.ReportData = await _reportRepository.GetReportDataAsync(
                flag,
                selectedProductId.Value,
                fromDate.Value,
                toDate.Value
            );

            model.ReportTitle = productName + " Report Summary";
            model.Dates = GetDateRange(fromDate.Value, toDate.Value);
            model.HasData = model.ReportData.Count > 0;

            return model;
        }

        private string GetProductName(List<ProductSetup> products, int productId)
        {
            int i = 0;

            while (i < products.Count)
            {
                if (products[i].ProductId == productId)
                {
                    return products[i].ProductName;
                }

                i++;
            }

            return "Monitoring";
        }

        private bool IsPingProduct(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return false;
            }

            productName = productName.Trim().ToLower();

            if (productName == "server ping")
            {
                return true;
            }

            return false;
        }

        private bool IsFortigateProduct(string productName)
        {
            if (string.IsNullOrWhiteSpace(productName))
            {
                return false;
            }

            productName = productName.Trim().ToLower();

            if (productName == "fortigate")
            {
                return true;
            }

            return false;
        }

        private List<DateTime> GetDateRange(DateTime fromDate, DateTime toDate)
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime currentDate = fromDate.Date;
            while (currentDate <= toDate.Date)
            {
                dates.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }

            return dates;
        }
    }
}