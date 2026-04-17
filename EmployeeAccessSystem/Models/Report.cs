using System;

namespace EmployeeAccessSystem.Models
{
    public class ReportModel
    {
        public string ProductName { get; set; }
        public string MonitoringTypeName { get; set; }
        public string ItemName { get; set; }
        public DateTime EntryDate { get; set; }
        public string ConfigValue { get; set; }
        public string EntryMode { get; set; }
        public bool IsChecked { get; set; }
        public int SN { get; set; }
        public string IPAddress { get; set; }
        public string ServerHostName { get; set; }
    }
}