using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class DepartmentService : IDepartmentService
{
  
    private static DepartmentService _instance;
    private static readonly object _lock = new object();


    private readonly DepartmentSQL _departmentSQL;


    private DepartmentService(string dbPath)
    {
        _departmentSQL = new DepartmentSQL(dbPath);
    }


    public static DepartmentService GetInstance(string dbPath)
    {
        if (_instance == null)
        {
            lock (_lock) // Ensure thread safety
            {
                if (_instance == null)
                {
                    _instance = new DepartmentService(dbPath);
                }
            }
        }
        return _instance;
    }

    public async Task<IEnumerable<Department>> GetAllDepartmentsAsync()
    {
        return await Task.Run(() => _departmentSQL.GetAllDepartments());
    }

    public async Task<Department> GetDepartmentByIdAsync(int departmentId)
    {
        return await Task.Run(() => _departmentSQL.GetDepartmentById(departmentId));
    }

    public async Task InsertDepartmentAsync(Department department)
    {
        await Task.Run(() => _departmentSQL.InsertDepartment(department));
    }

    public async Task UpdateDepartmentAsync(Department department)
    {
        await Task.Run(() => _departmentSQL.UpdateDepartment(department));
    }

    public async Task DeleteDepartmentAsync(int departmentId)
    {
        await Task.Run(() => _departmentSQL.DeleteDepartment(departmentId));
    }
}
