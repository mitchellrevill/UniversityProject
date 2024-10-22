﻿using Microsoft.Data.Sqlite;
using System;
using UniversityProject.Model;
using Microsoft.Data.Sqlite;

namespace UniversityProject.Data
{
    public class EmployeeDatabase
    {
        private readonly string _connectionString;

        public EmployeeDatabase(string dbPath)
        {
            _connectionString = $"Data Source={dbPath}";
            CreateTable();
        }

        // Create the Employee table if it does not exist
        // Create the Employee table if it does not exist
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
                CountryId TEXT NOT NULL,
                DepartmentId TEXT NOT NULL,
                ManagerId TEXT,
                RegionId TEXT NOT NULL,
                EmploymentType TEXT,
                StartDate TEXT NOT NULL,
                Salary REAL NOT NULL,
                Benefits TEXT,
                Status INTEGER NOT NULL
            )";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        // Method to insert a new employee
        public void InsertEmployee(Employee employee)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
            INSERT INTO Employees (EmployeeId, FirstName, LastName, CompanyEmail, PersonalEmail, PhoneNumber, CountryId, DepartmentId, ManagerId, RegionId, EmploymentType, StartDate, Salary, Benefits, Status)
            VALUES (@EmployeeId, @FirstName, @LastName, @CompanyEmail, @PersonalEmail, @PhoneNumber, @CountryId, @DepartmentId, @ManagerId, @RegionId, @EmploymentType, @StartDate, @Salary, @Benefits, @Status)";

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
                    command.Parameters.AddWithValue("@ManagerId", employee.ManagerId);
                    command.Parameters.AddWithValue("@RegionId", employee.RegionId);
                    command.Parameters.AddWithValue("@EmploymentType", employee.EmploymentType);
                    command.Parameters.AddWithValue("@StartDate", employee.StartDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Salary", employee.Salary);
                    command.Parameters.AddWithValue("@Benefits", employee.Benefits);
                    command.Parameters.AddWithValue("@Status", (int)employee.Status); // Convert enum to int

                    command.ExecuteNonQuery();
                }
            }
        }

        // Method to retrieve all employees
        public void GetAllEmployees()
        {
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
                            CountryId = reader.GetString(6),
                            DepartmentId = reader.GetString(7),
                            ManagerId = reader.IsDBNull(8) ? null : reader.GetString(8),
                            RegionId = reader.GetString(9),
                            EmploymentType = reader.GetString(10),
                            StartDate = DateTime.Parse(reader.GetString(11)),
                            Salary = reader.GetDecimal(12),
                            Benefits = reader.GetString(13),
                            Status = (Employee.EmployeeStatus)reader.GetInt32(14) // Convert int back to enum
                        };

                        Console.WriteLine($"ID: {employee.EmployeeId}, Name: {employee.FirstName} {employee.LastName}, Company Email: {employee.CompanyEmail}, Personal Email: {employee.PersonalEmail}, Phone: {employee.PhoneNumber}, Country: {employee.CountryId}, Department: {employee.DepartmentId}, Manager: {employee.ManagerId}, Region: {employee.RegionId}, Employment Type: {employee.EmploymentType}, Start Date: {employee.StartDate}, Salary: {employee.Salary}, Benefits: {employee.Benefits}, Status: {employee.Status}");
                    }
                }
            }
        }
    }
}
