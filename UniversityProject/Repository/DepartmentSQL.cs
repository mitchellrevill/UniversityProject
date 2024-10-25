using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class DepartmentSQL
    {
        private readonly string _connectionString;

        public DepartmentSQL(string dbPath)
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
                CREATE TABLE IF NOT EXISTS Departments (
                    DepartmentId INTEGER PRIMARY KEY,
                    DepartmentName TEXT NOT NULL,
                    Description TEXT
                )";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertDepartment(Department department)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                INSERT INTO Departments (DepartmentName, Description)
                VALUES (@DepartmentName, @Description)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    command.Parameters.AddWithValue("@Description", department.Description);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Department> GetAllDepartments()
        {
            var departments = new List<Department>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Departments";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var department = new Department
                        {
                            DepartmentId = reader.GetInt32(0),
                            DepartmentName = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                        };

                        departments.Add(department);
                    }
                }
            }
            return departments;
        }

        public Department GetDepartmentById(int departmentId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Departments WHERE DepartmentId = @DepartmentId";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentId", departmentId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Department
                            {
                                DepartmentId = reader.GetInt32(0),
                                DepartmentName = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void UpdateDepartment(Department department)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE Departments
                SET DepartmentName = @DepartmentName,
                    Description = @Description
                WHERE DepartmentId = @DepartmentId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    command.Parameters.AddWithValue("@Description", department.Description);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Departments WHERE DepartmentId = @DepartmentId";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@DepartmentId", departmentId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
