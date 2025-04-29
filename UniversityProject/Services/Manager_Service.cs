using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class ManagerService : IManagerService
{
    // Singleton instance
    private static ManagerService _instance;


    private static readonly object _lock = new object();

    private readonly ManagerSQL _managerSQL;

 
    private ManagerService(string dbPath)
    {
        _managerSQL = new ManagerSQL(dbPath);
    }

    public static ManagerService GetInstance(string dbPath)
    {
        if (_instance == null)
        {
            lock (_lock) 
            {
                if (_instance == null)
                {
                    _instance = new ManagerService(dbPath);
                }
            }
        }
        return _instance;
    }

    // Methods remain unchanged
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
