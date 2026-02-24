using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subRepo;

        public SubCategoryService(ISubCategoryRepository subRepo)
        {
            _subRepo = subRepo;
        }

        public async Task CreateAsync(SubCategory model)
        {
            await _subRepo.AddAsync(model);
        }

        public async Task UpdateAsync(SubCategory model)
        {
            await _subRepo.UpdateAsync(model);
        }

        public async Task DeleteAsync(int id)
        {
            await _subRepo.DeleteAsync(id);
        }
    }
}