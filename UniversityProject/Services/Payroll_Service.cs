using UniversityProject.Model;
using UniversityProject.Repository;
using UniversityProject.Interfaces;

public class PayrollService : IPayrollService
{
    private readonly PayrollSQL _applicationDatabase;

    public PayrollService(string dbPath)
    {
        _applicationDatabase = new PayrollSQL(dbPath);
    }

    public async Task<IEnumerable<Payroll>> GetAllPayrollsAsync()
    {
        return await Task.Run(() => _applicationDatabase.GetAllPayrolls());
    }
    public async Task<IEnumerable<Payroll>> GetAllPayrollsByIdAsync(string employeeId)
    {
        return await Task.Run(() => _applicationDatabase.GetAllPayrollsById(employeeId));
    }
    public async Task InsertPayrollAsync(Payroll Payroll)
    {
        await Task.Run(() => _applicationDatabase.InsertPayroll(Payroll));
    }

    public async Task UpdatePayrollAsync(Payroll Payroll)
    {
        await Task.Run(() => _applicationDatabase.UpdatePayroll(Payroll));
    }

    public async Task DeletePayrollAsync(int PayrollId)
    {
        await Task.Run(() => _applicationDatabase.DeletePayroll(PayrollId));
    }

  
}
