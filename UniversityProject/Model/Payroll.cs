using System;
using System.Collections.Generic;

namespace UniversityProject.Model
{
    public class Payroll
    {
        public int PayrollId { get; set; }
        public int EmployeeId { get; set; }
        public string TaxNumberCode { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal ThisPaycheck { get; set; }
        public string Recurrence { get; set; }
        public decimal Tax { get; set; }
        public decimal ExtraDeductions { get; set; }
        public DateTime PayPeriodStart { get; set; }
        public DateTime PayPeriodEnd { get; set; }
        public decimal NetPay { get; set; }
        public decimal Bonuses { get; set; }
        public int OvertimeHours { get; set; }
        public decimal OvertimePay { get; set; }
        public List<string> Deductions { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }

        public Payroll()
        {
            Deductions = new List<string>();
        }
    }
}
