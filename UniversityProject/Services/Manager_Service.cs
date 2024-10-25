using System.Collections.Generic;
using System.Threading.Tasks;
using UniversityProject.Model;
using UniversityProject.Repository;
using UniversityProject.Interfaces;

public class ManagerService : IManagerService
{
    private readonly ManagerSQL _managerSQL;

    public ManagerService(string dbPath)
    {
        _managerSQL = new ManagerSQL(dbPath);
    }

    public async Task<IEnumerable<Manager>> GetAllManagersAsync()
    {
        return await Task.Run(() => _managerSQL.GetAllManagers());
    }

    public async Task<Manager> GetManagerByIdAsync(string employeeId)
    {
        return await Task.Run(() => _managerSQL.GetManagerById(employeeId));
    }

    public async Task InsertManagerAsync(Manager manager)
    {
        await Task.Run(() => _managerSQL.InsertManager(manager));
    }

    public async Task UpdateManagerAsync(Manager manager)
    {
        await Task.Run(() => _managerSQL.UpdateManager(manager));
    }

    public async Task DeleteManagerAsync(string employeeId)
    {
        await Task.Run(() => _managerSQL.DeleteManager(employeeId));
    }
}
