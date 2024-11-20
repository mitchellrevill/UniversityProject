using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class PayrollSQL
    {
        private readonly string _connectionString;

        public PayrollSQL(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            CreateTable();
        }

        private void CreateTable()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Payrolls (
                    PayrollId INTEGER PRIMARY KEY AUTOINCREMENT,
                    EmployeeId INTEGER NOT NULL,
                    TaxNumberCode TEXT NOT NULL,
                    BaseSalary REAL NOT NULL,
                    ThisPaycheck REAL NOT NULL,
                    Recurrence TEXT NOT NULL,
                    Tax REAL NOT NULL,
                    ExtraDeductions REAL NOT NULL,
                    PayPeriodStart TEXT NOT NULL,
                    PayPeriodEnd TEXT NOT NULL,
                    NetPay REAL NOT NULL,
                    Bonuses REAL NOT NULL,
                    OvertimeHours INTEGER NOT NULL,
                    OvertimePay REAL NOT NULL,
                    PaymentDate TEXT NOT NULL,
                    PaymentMethod TEXT NOT NULL,
                    Deductions TEXT
                );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertPayroll(Payroll payroll)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string insertQuery = @"
                INSERT INTO Payrolls (EmployeeId, TaxNumberCode, BaseSalary, ThisPaycheck, Recurrence, Tax, ExtraDeductions, PayPeriodStart, PayPeriodEnd, NetPay, Bonuses, OvertimeHours, OvertimePay, PaymentDate, PaymentMethod, Deductions)
                VALUES (@EmployeeId, @TaxNumberCode, @BaseSalary, @ThisPaycheck, @Recurrence, @Tax, @ExtraDeductions, @PayPeriodStart, @PayPeriodEnd, @NetPay, @Bonuses, @OvertimeHours, @OvertimePay, @PaymentDate, @PaymentMethod, @Deductions)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", payroll.EmployeeId);
                    command.Parameters.AddWithValue("@TaxNumberCode", payroll.TaxNumberCode);
                    command.Parameters.AddWithValue("@BaseSalary", payroll.BaseSalary);
                    command.Parameters.AddWithValue("@ThisPaycheck", payroll.ThisPaycheck);
                    command.Parameters.AddWithValue("@Recurrence", payroll.Recurrence);
                    command.Parameters.AddWithValue("@Tax", payroll.Tax);
                    command.Parameters.AddWithValue("@ExtraDeductions", payroll.ExtraDeductions);
                    command.Parameters.AddWithValue("@PayPeriodStart", payroll.PayPeriodStart.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@PayPeriodEnd", payroll.PayPeriodEnd.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@NetPay", payroll.NetPay);
                    command.Parameters.AddWithValue("@Bonuses", payroll.Bonuses);
                    command.Parameters.AddWithValue("@OvertimeHours", payroll.OvertimeHours);
                    command.Parameters.AddWithValue("@OvertimePay", payroll.OvertimePay);
                    command.Parameters.AddWithValue("@PaymentDate", payroll.PaymentDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@PaymentMethod", payroll.PaymentMethod);
                    command.Parameters.AddWithValue("@Deductions", string.Join(",", payroll.Deductions));
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Payroll> GetAllPayrolls()
        {
            var payrolls = new List<Payroll>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Payrolls";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var payroll = new Payroll
                        {
                            PayrollId = reader.GetInt32(0),
                            EmployeeId = reader.GetInt32(1),
                            TaxNumberCode = reader.GetString(2),
                            BaseSalary = reader.GetDecimal(3),
                            ThisPaycheck = reader.GetDecimal(4),
                            Recurrence = reader.GetString(5),
                            Tax = reader.GetDecimal(6),
                            ExtraDeductions = reader.GetDecimal(7),
                            PayPeriodStart = DateTime.Parse(reader.GetString(8)),
                            PayPeriodEnd = DateTime.Parse(reader.GetString(9)),
                            NetPay = reader.GetDecimal(10),
                            Bonuses = reader.GetDecimal(11),
                            OvertimeHours = reader.GetInt32(12),
                            OvertimePay = reader.GetDecimal(13),
                            PaymentDate = DateTime.Parse(reader.GetString(14)),
                            PaymentMethod = reader.GetString(15),
                            Deductions = new List<string>(reader.GetString(16).Split(',')),
                        };

                        payrolls.Add(payroll);
                    }
                }
            }

            return payrolls;
        }

        public Payroll GetPayrollById(int payrollId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Payrolls WHERE PayrollId = @PayrollId";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@PayrollId", payrollId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Payroll
                            {
                                PayrollId = reader.GetInt32(0),
                                EmployeeId = reader.GetInt32(1),
                                TaxNumberCode = reader.GetString(2),
                                BaseSalary = reader.GetDecimal(3),
                                ThisPaycheck = reader.GetDecimal(4),
                                Recurrence = reader.GetString(5),
                                Tax = reader.GetDecimal(6),
                                ExtraDeductions = reader.GetDecimal(7),
                                PayPeriodStart = DateTime.Parse(reader.GetString(8)),
                                PayPeriodEnd = DateTime.Parse(reader.GetString(9)),
                                NetPay = reader.GetDecimal(10),
                                Bonuses = reader.GetDecimal(11),
                                OvertimeHours = reader.GetInt32(12),
                                OvertimePay = reader.GetDecimal(13),
                                PaymentDate = DateTime.Parse(reader.GetString(14)),
                                PaymentMethod = reader.GetString(15),
                                Deductions = new List<string>(reader.GetString(16).Split(',')),
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void UpdatePayroll(Payroll payroll)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE Payrolls
                SET EmployeeId = @EmployeeId,
                    TaxNumberCode = @TaxNumberCode,
                    BaseSalary = @BaseSalary,
                    ThisPaycheck = @ThisPaycheck,
                    Recurrence = @Recurrence,
                    Tax = @Tax,
                    ExtraDeductions = @ExtraDeductions,
                    PayPeriodStart = @PayPeriodStart,
                    PayPeriodEnd = @PayPeriodEnd,
                    NetPay = @NetPay,
                    Bonuses = @Bonuses,
                    OvertimeHours = @OvertimeHours,
                    OvertimePay = @OvertimePay,
                    PaymentDate = @PaymentDate,
                    PaymentMethod = @PaymentMethod,
                    Deductions = @Deductions
                WHERE PayrollId = @PayrollId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@PayrollId", payroll.PayrollId);
                    command.Parameters.AddWithValue("@EmployeeId", payroll.EmployeeId);
                    command.Parameters.AddWithValue("@TaxNumberCode", payroll.TaxNumberCode);
                    command.Parameters.AddWithValue("@BaseSalary", payroll.BaseSalary);
                    command.Parameters.AddWithValue("@ThisPaycheck", payroll.ThisPaycheck);
                    command.Parameters.AddWithValue("@Recurrence", payroll.Recurrence);
                    command.Parameters.AddWithValue("@Tax", payroll.Tax);
                    command.Parameters.AddWithValue("@ExtraDeductions", payroll.ExtraDeductions);
                    command.Parameters.AddWithValue("@PayPeriodStart", payroll.PayPeriodStart.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@PayPeriodEnd", payroll.PayPeriodEnd.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@NetPay", payroll.NetPay);
                    command.Parameters.AddWithValue("@Bonuses", payroll.Bonuses);
                    command.Parameters.AddWithValue("@OvertimeHours", payroll.OvertimeHours);
                    command.Parameters.AddWithValue("@OvertimePay", payroll.OvertimePay);
                    command.Parameters.AddWithValue("@PaymentDate", payroll.PaymentDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@PaymentMethod", payroll.PaymentMethod);
                    command.Parameters.AddWithValue("@Deductions", string.Join(",", payroll.Deductions));
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeletePayroll(int payrollId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Payrolls WHERE PayrollId = @PayrollId";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@PayrollId", payrollId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
