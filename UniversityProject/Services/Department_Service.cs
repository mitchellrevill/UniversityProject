using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class DepartmentService : IDepartmentService
{
    private readonly DepartmentSQL _departmentSQL;

    public DepartmentService(string dbPath)
    {
        _departmentSQL = new DepartmentSQL(dbPath);
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
