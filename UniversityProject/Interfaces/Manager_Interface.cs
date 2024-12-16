using UniversityProject.Model;

namespace UniversityProject.Interfaces
{
    public interface IManagerService
    {
        Task<IEnumerable<Manager>> GetAllManagersAsync();
        Task<Manager> GetManagerByIdAsync(string employeeId);
        Task InsertManagerAsync(Manager manager);
        Task UpdateManagerAsync(Manager manager);
        Task DeleteManagerAsync(string employeeId);
    }
}
