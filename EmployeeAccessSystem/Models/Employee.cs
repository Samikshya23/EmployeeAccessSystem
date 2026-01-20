namespace EmployeeAccessSystem.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        public string? FullName { get; set; }
        public string? Email { get; set; }

        public int DepartmentId { get; set; }

       
        public string? DepartmentName { get; set; }

        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
