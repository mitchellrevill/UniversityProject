﻿using UniversityProject.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversityProject.Interfaces
{
    public interface IPayrollService
    {
        Task<IEnumerable<Payroll>> GetAllPayrollsAsync();
        Task<IEnumerable<Payroll>> GetAllPayrollsByIdAsync(string employeeId);
        Task InsertPayrollAsync(Payroll Payroll);
        Task UpdatePayrollAsync(Payroll Payroll);
        Task DeletePayrollAsync(int PayrollId);
    }
}
