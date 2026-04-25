using System.Collections.Generic;

namespace EmployeeAccessSystem.Models
{
    public class ProductConfigurationIndexItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";

        public List<ProductConfiguration> Nodes { get; set; } = new();
    }
}