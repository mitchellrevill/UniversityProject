using Microsoft.Data.Sqlite;
using System;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class EmployeeSQL
    {
        private readonly string _connectionString;

        public EmployeeSQL(string dbPath)
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
                CREATE TABLE IF NOT EXISTS Employees (
                    EmployeeId TEXT PRIMARY KEY,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    CompanyEmail TEXT NOT NULL,
                    PersonalEmail TEXT NOT NULL,
                    PhoneNumber TEXT NOT NULL,
                    CountryId INTEGER NOT NULL,
                    DepartmentId INTEGER NOT NULL,
                    ManagerId INTEGER,
                    RegionId INTEGER NOT NULL,
                    EmploymentType TEXT,
                    StartDate TEXT NOT NULL,
                    Salary REAL NOT NULL,
                    Benefits TEXT,
                    Employeetype TEXT,
                    Password TEXT,
                    Status INTEGER NOT NULL,
                    FOREIGN KEY (CountryId) REFERENCES Country(CountryId) ON DELETE CASCADE,
                    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId) ON DELETE SET NULL,
                    FOREIGN KEY (ManagerId) REFERENCES Managers(ManagerId) ON DELETE SET NULL,
                    FOREIGN KEY (RegionId) REFERENCES Regions(RegionId) ON DELETE SET NULL
                );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        public void InsertEmployee(Employee employee)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string insertQuery = @"
                INSERT INTO Employees (EmployeeId, FirstName, LastName, CompanyEmail, PersonalEmail, PhoneNumber, CountryId, DepartmentId, ManagerId, RegionId, EmploymentType, StartDate, Salary, Benefits, Employeetype, Password, Status)
                VALUES (@EmployeeId, @FirstName, @LastName, @CompanyEmail, @PersonalEmail, @PhoneNumber, @CountryId, @DepartmentId, @ManagerId, @RegionId, @EmploymentType, @StartDate, @Salary, @Benefits, @Employeetype, @Password, @Status)";
                Console.WriteLine("EmployeeId: " + employee.EmployeeId);
                Console.WriteLine("FirstName: " + employee.FirstName);
                Console.WriteLine("LastName: " + employee.LastName);
                Console.WriteLine("CompanyEmail: " + employee.CompanyEmail);
                Console.WriteLine("PersonalEmail: " + employee.PersonalEmail);
                Console.WriteLine("PhoneNumber: " + employee.PhoneNumber);
                Console.WriteLine("CountryId: " + employee.CountryId);
                Console.WriteLine("DepartmentId: " + employee.DepartmentId);
                Console.WriteLine("ManagerId: " + employee.ManagerId);
                Console.WriteLine("RegionId: " + employee.RegionId);
                Console.WriteLine("EmploymentType: " + employee.EmploymentType);
                Console.WriteLine("StartDate: " + employee.StartDate.ToString("yyyy-MM-dd"));
                Console.WriteLine("Salary: " + employee.Salary);
                Console.WriteLine("Benefits: " + employee.Benefits);
                Console.WriteLine("Employeetype: " + employee.Employeetype);
                Console.WriteLine("Password: " + employee.password);
                Console.WriteLine("Status: " + employee.Status);

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                    command.Parameters.AddWithValue("@LastName", employee.LastName);
                    command.Parameters.AddWithValue("@CompanyEmail", employee.CompanyEmail);
                    command.Parameters.AddWithValue("@PersonalEmail", employee.PersonalEmail);
                    command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                    command.Parameters.AddWithValue("@CountryId", employee.CountryId);
                    command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
                    command.Parameters.AddWithValue("@ManagerId", (object)employee.ManagerId ?? DBNull.Value); 
                    command.Parameters.AddWithValue("@RegionId", employee.RegionId);
                    command.Parameters.AddWithValue("@EmploymentType", employee.EmploymentType);
                    command.Parameters.AddWithValue("@StartDate", employee.StartDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@Benefits", employee.Benefits);
                    command.Parameters.AddWithValue("@Employeetype", employee.Employeetype);
                    command.Parameters.AddWithValue("@Password", employee.password);
                    command.Parameters.AddWithValue("@Status", (int)employee.Status);
                    command.ExecuteNonQuery();
                }

            }
        }
        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Employees";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var employee = new Employee
                        {
                            EmployeeId = reader.GetString(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            CompanyEmail = reader.GetString(3),
                            PersonalEmail = reader.GetString(4),
                            PhoneNumber = reader.GetString(5),
                            CountryId = reader.GetInt32(6),
                            DepartmentId = reader.GetInt32(7),
                            ManagerId = reader.IsDBNull(8) ? (int?)null : reader.GetInt32(8), // Handle nullable ManagerId
                            RegionId = reader.GetInt32(9),
                            EmploymentType = reader.GetString(10),
                            StartDate = DateTime.Parse(reader.GetString(11)),
                            Salary = reader.GetInt32(12),
                            Benefits = reader.GetString(13),
                            Employeetype = reader.GetString(14),
                            password = reader.GetString(15),
                            Status = (Employee.EmployeeStatus)reader.GetInt32(16)
                        };

                        employees.Add(employee);
                    }
                }
            }

            return employees;
        }

        public Employee GetEmployeeById(string employeeId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Employee
                            {
                                EmployeeId = reader.GetString(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                CompanyEmail = reader.GetString(3),
                                PersonalEmail = reader.GetString(4),
                                PhoneNumber = reader.GetString(5),
                                CountryId = reader.GetInt32(6),
                                DepartmentId = reader.GetInt32(7),
                                ManagerId = reader.GetInt32(8),
                                RegionId = reader.GetInt32(9),
                                EmploymentType = reader.GetString(10),
                                StartDate = DateTime.Parse(reader.GetString(11)),
                                Salary = reader.GetInt32(12),
                                Benefits = reader.GetString(13),
                                Employeetype = reader.GetString(14),
                                password = reader.GetString(15),
                                Status = (Employee.EmployeeStatus)reader.GetInt32(16)
                            };
                        }
                    }
                }
            }
            return null;
        }
 

        public void UpdateEmployee(Employee employee)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE Employees
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    CompanyEmail = @CompanyEmail,
                    PersonalEmail = @PersonalEmail,
                    PhoneNumber = @PhoneNumber,
                    CountryId = @CountryId,
                    DepartmentId = @DepartmentId,
                    ManagerId = @ManagerId,
                    RegionId = @RegionId,
                    EmploymentType = @EmploymentType,
                    StartDate = @StartDate,
                    Salary = @Salary,
                    Benefits = @Benefits,
                    Employeetype = @Employeetype,
                    Password = @Password,
                    Status = @Status
                WHERE EmployeeId = @EmployeeId";


                using (var command = new SqliteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                        command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                        command.Parameters.AddWithValue("@LastName", employee.LastName);
                        command.Parameters.AddWithValue("@CompanyEmail", employee.CompanyEmail);
                        command.Parameters.AddWithValue("@PersonalEmail", employee.PersonalEmail);
                        command.Parameters.AddWithValue("@PhoneNumber", employee.PhoneNumber);
                        command.Parameters.AddWithValue("@CountryId", employee.CountryId);
                        command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
                        command.Parameters.AddWithValue("@ManagerId", (object)employee.ManagerId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@RegionId", employee.RegionId);
                        command.Parameters.AddWithValue("@EmploymentType", employee.EmploymentType);
                        command.Parameters.AddWithValue("@StartDate", employee.StartDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Salary", employee.Salary);
                        command.Parameters.AddWithValue("@Benefits", employee.Benefits);
                        command.Parameters.AddWithValue("@Employeetype", employee.Employeetype);
                        command.Parameters.AddWithValue("@Password", employee.password);
                        command.Parameters.AddWithValue("@Status", (int)employee.Status);
                        command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteEmployee(string employeeId)
        {   
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}