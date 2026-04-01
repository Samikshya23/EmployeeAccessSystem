using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeAccessSystem.Services
{
    public class ProductSetupService : IProductSetupService
    {
        private readonly IProductSetupRepositories _repo;

        public ProductSetupService(IProductSetupRepositories repo)
        {
            _repo = repo;
        }
        public async Task<List<ProductSetup>> GetAllAsync()
        {
            var data = await _repo.GetAllAsync();
            return data.ToList();
        }
        public Task<ProductSetup?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public Task AddAsync(ProductSetup productSetup)
        {
            return _repo.AddAsync(productSetup);
        }

        public Task UpdateAsync(ProductSetup productSetup)
        {
            return _repo.UpdateAsync(productSetup);
        }

        public Task DeleteAsync(int id)
        {
            return _repo.DeleteAsync(id);
        }

        public Task ToggleAsync(int id)
        {
            return _repo.ToggleAsync(id);
        }
    }
}