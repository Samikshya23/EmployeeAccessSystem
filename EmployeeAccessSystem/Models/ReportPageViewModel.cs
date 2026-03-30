using System;
using System.Collections.Generic;

namespace EmployeeAccessSystem.Models
{
    public class ReportPageViewModel
    {
        public List<ProductSetup> ProductList { get; set; }
        public int? SelectedProductId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string ReportTitle { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<ReportModel> ReportData { get; set; }
        public bool HasData { get; set; }

        public ReportPageViewModel()
        {
            ProductList = new List<ProductSetup>();
            Dates = new List<DateTime>();
            ReportData = new List<ReportModel>();
            ReportTitle = "Monitoring Report";
        }
    }
}