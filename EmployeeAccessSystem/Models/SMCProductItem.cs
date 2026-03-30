namespace EmployeeAccessSystem.Models
{
    public class SMCProductItem
    {
        public int SMCProductItemId { get; set; }
        public int SMCProductId { get; set; }
        public string ItemName { get; set; }
        //public string ValueType { get; set; }
        public bool IsActive { get; set; }
        public string? SMCProductName { get; set; }
    }
}