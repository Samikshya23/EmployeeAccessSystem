using EmployeeAccessSystem.Models;
using EmployeeAccessSystem.Repositories;

namespace EmployeeAccessSystem.Services
{
    public class SMCConfigService : ISMCConfigService
    {
        private readonly ISMCConfigRepository _repo;

        public SMCConfigService(ISMCConfigRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SMCConfig>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<SMCConfig> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<int> AddAsync(SMCConfig model)
        {
            return await _repo.AddAsync(model);
        }

        public async Task<int> UpdateAsync(SMCConfig model)
        {
            return await _repo.UpdateAsync(model);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task ToggleAsync(int id)
        {
            await _repo.ToggleAsync(id);
        }
    }
}