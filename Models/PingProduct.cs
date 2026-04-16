using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessSystem.Models
{
    public class PingProduct
    {
        public int PingProductId { get; set; }

        [Required(ErrorMessage = "Please select a product.")]
        public int ProductId { get; set; }

        public int SN { get; set; }

        [Required(ErrorMessage = "Please enter IP address.")]
        [StringLength(100, ErrorMessage = "IP Address cannot exceed 100 characters.")]
        public string IPAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter server host name.")]
        [StringLength(200, ErrorMessage = "Server Host Name cannot exceed 200 characters.")]
        public string ServerHostName { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;
    }
}