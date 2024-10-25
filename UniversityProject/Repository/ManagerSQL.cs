using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class ManagerSQL
    {
        private readonly string _connectionString;

        public ManagerSQL(string dbPath)
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
                CREATE TABLE IF NOT EXISTS Managers (
                    EmployeeId TEXT PRIMARY KEY,
                    ManagerArea TEXT NOT NULL,
                    ManagerId TEXT NOT NULL
                )";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertManager(Manager manager)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                INSERT INTO Managers (EmployeeId, ManagerArea, ManagerId)
                VALUES (@EmployeeId, @ManagerArea, @ManagerId)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", manager.EmployeeId);
                    command.Parameters.AddWithValue("@ManagerArea", manager.ManagerArea);
                    command.Parameters.AddWithValue("@ManagerId", manager.ManagerId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Manager> GetAllManagers()
        {
            var managers = new List<Manager>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Managers";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        managers.Add(new Manager
                        {
                            EmployeeId = reader.GetString(0),
                            ManagerArea = reader.GetString(1),
                            ManagerId = reader.GetString(2)
                        });
                    }
                }
            }
            return managers;
        }

        public Manager GetManagerById(string employeeId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Managers WHERE EmployeeId = @EmployeeId";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Manager
                            {
                                EmployeeId = reader.GetString(0),
                                ManagerArea = reader.GetString(1),
                                ManagerId = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void UpdateManager(Manager manager)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE Managers
                SET ManagerArea = @ManagerArea,
                    ManagerId = @ManagerId
                WHERE EmployeeId = @EmployeeId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", manager.EmployeeId);
                    command.Parameters.AddWithValue("@ManagerArea", manager.ManagerArea);
                    command.Parameters.AddWithValue("@ManagerId", manager.ManagerId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteManager(string employeeId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Managers WHERE EmployeeId = @EmployeeId";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
