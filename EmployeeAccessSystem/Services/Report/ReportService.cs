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

            model.ProductList = (await _productSetupService.GetAllAsync()).ToList();

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

            model.ReportData = await _reportRepository.GetReportDataAsync(
                selectedProductId.Value,
                fromDate.Value,
                toDate.Value
            );

            model.ReportTitle = GetProductName(model.ProductList, selectedProductId.Value) + " Report Summary";
            model.Dates = GetUniqueDates(model.ReportData);
            model.HasData = model.ReportData.Count > 0;

            return model;
        }

        private string GetProductName(List<ProductSetup> products, int productId)
        {
            for (int i = 0; i < products.Count; i++)
            {
                if (products[i].ProductId == productId)
                {
                    return products[i].ProductName;
                }
            }

            return "Monitoring";
        }

        private List<DateTime> GetUniqueDates(List<ReportModel> data)
        {
            List<DateTime> dates = new List<DateTime>();

            for (int i = 0; i < data.Count; i++)
            {
                DateTime currentDate = data[i].EntryDate.Date;
                bool exists = false;

                for (int j = 0; j < dates.Count; j++)
                {
                    if (dates[j] == currentDate)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    dates.Add(currentDate);
                }
            }

            dates.Sort();
            return dates;
        }
    }
}