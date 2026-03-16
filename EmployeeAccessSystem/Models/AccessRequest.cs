namespace EmployeeAccessSystem.Models
{
    public class AccessRequest
    {
        public int RequestId { get; set; }

        public int EmployeeId { get; set; }

        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        public string? RequestReason { get; set; }

        public DateTime RequestDate { get; set; }

        public string SupervisorStatus { get; set; } = "Pending";

        public string? SupervisorComment { get; set; }

        public int? SupervisorActionByEmployeeId { get; set; }

        public DateTime? SupervisorActionDate { get; set; }

        public string AdminStatus { get; set; } = "Pending";

        public string? AdminComment { get; set; }

        public int? AdminActionByEmployeeId { get; set; }

        public DateTime? AdminActionDate { get; set; }

        public string FinalStatus { get; set; } = "Pending";
    }
}