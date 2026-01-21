using EmployeeAccessSystem.Models;

namespace EmployeeAccessSystem.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        void ToggleActive(int id);
    }
}
