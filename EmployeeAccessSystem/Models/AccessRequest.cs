using System;

namespace EmployeeAccessSystem.Models
{
    public class AccessRequest
    {
        public int RequestId { get; set; }
        public int EmployeeId { get; set; }
        public int CategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public string? AssetTag { get; set; }
        public string? Duration { get; set; }
        public string? RequestReason { get; set; }
        public DateTime RequestDate { get; set; }
        public string SupervisorStatus { get; set; } = "Pending";
        public string? SupervisorComment { get; set; }
        public int? SupervisorActionByEmployeeId { get; set; }
        public DateTime? SupervisorActionDate { get; set; }
        public string FinalStatus { get; set; } = "Pending";
        public string? EmployeeName { get; set; }
        public string? CategoryName { get; set; }
        public string? ServerName { get; set; }
        public string? ServerIP { get; set; }
    }
}