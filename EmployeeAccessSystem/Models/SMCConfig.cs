namespace EmployeeAccessSystem.Models
{
    public class SMCConfig
    {
        public int SMCConfigId { get; set; }

        public int ProductId { get; set; }
        public int SMCProductId { get; set; }
        public int SMCProductItemId { get; set; }

        public DateTime EntryDate { get; set; }

        public string ConfigValue { get; set; }
        public string? Remarks { get; set; }

        public bool IsActive { get; set; }

       
        public string? ProductName { get; set; }
        public string? SMCProductName { get; set; }
        public string? ItemName { get; set; }
    }
}