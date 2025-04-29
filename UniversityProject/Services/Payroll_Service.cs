using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class PayrollService : IPayrollService
{
    // Singleton instance
    private static PayrollService _instance;


    private static readonly object _lock = new object();

    private readonly PayrollSQL _applicationDatabase;

  
    private PayrollService(string dbPath)
    {
        _applicationDatabase = new PayrollSQL(dbPath);
    }


    public static PayrollService GetInstance(string dbPath)
    {
        if (_instance == null)
        {
            lock (_lock) // Ensure thread safety
            {
                if (_instance == null)
                {
                    _instance = new PayrollService(dbPath);
                }
            }
        }
        return _instance;
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
