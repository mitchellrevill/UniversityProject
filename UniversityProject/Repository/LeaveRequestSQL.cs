using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class LeaveRequestRepository
    {
        private readonly string _connectionString;

        public LeaveRequestRepository(string dbPath)
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
                CREATE TABLE IF NOT EXISTS LeaveRequests (
                    LeaveRequestId INTEGER PRIMARY KEY AUTOINCREMENT,
                    EmployeeId TEXT NOT NULL,
                    StartDate TEXT NOT NULL,
                    EndDate TEXT NOT NULL,
                    HoursUsed INTEGER NOT NULL,
                    IsApproved TEXT NOT NULL,
                    FOREIGN KEY (EmployeeId) REFERENCES Employees (EmployeeId) ON DELETE CASCADE
                );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        public void InsertLeaveRequest(LeaveRequest leaveRequest)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string insertQuery = @"
                INSERT INTO LeaveRequests (EmployeeId, StartDate, EndDate, HoursUsed, IsApproved)
                VALUES (@EmployeeId, @StartDate, @EndDate, @HoursUsed, @IsApproved);";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", leaveRequest.EmployeeId);
                    command.Parameters.AddWithValue("@StartDate", leaveRequest.StartDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@EndDate", leaveRequest.EndDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@HoursUsed", leaveRequest.HoursUsed);
                    command.Parameters.AddWithValue("@IsApproved", leaveRequest.IsApproved);
                    command.ExecuteNonQuery();
                }
            }
        }
        public List<LeaveRequest> GetAllLeaveRequests()
        {
            var leaveRequests = new List<LeaveRequest>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM LeaveRequests";

                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        leaveRequests.Add(new LeaveRequest
                        {
                            LeaveRequestId = reader.GetInt32(0),
                            EmployeeId = reader.GetString(1),
                            StartDate = DateTime.Parse(reader.GetString(2)),
                            EndDate = DateTime.Parse(reader.GetString(3)),
                            HoursUsed = reader.GetInt32(4),
                            IsApproved = reader.GetString(5) 
                        });
                    }
                }
            }

            return leaveRequests;
        }

        public LeaveRequest GetLeaveRequestByEmployeeId(int employeeId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT * FROM LeaveRequests WHERE EmployeeId = @EmployeeId";

                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeId", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new LeaveRequest
                            {
                                LeaveRequestId = reader.GetInt32(0),
                                EmployeeId = reader.GetString(1),
                                StartDate = DateTime.Parse(reader.GetString(2)),
                                EndDate = DateTime.Parse(reader.GetString(3)),
                                HoursUsed = reader.GetInt32(4),
                                IsApproved = reader.GetString(5)
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void UpdateLeaveRequest(LeaveRequest leaveRequest)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string updateQuery = @"
        UPDATE LeaveRequests
        SET HoursUsed = @HoursUsed, IsApproved = @IsApproved
        WHERE LeaveRequestId = @LeaveRequestId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@LeaveRequestId", leaveRequest.LeaveRequestId);
                    command.Parameters.AddWithValue("@HoursUsed", leaveRequest.HoursUsed);
                    command.Parameters.AddWithValue("@IsApproved", leaveRequest.IsApproved);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteLeaveRequest(int leaveRequestId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM LeaveRequests WHERE LeaveRequestId = @LeaveRequestId";

                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@LeaveRequestId", leaveRequestId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void ApproveLeave(int leaveRequestId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string approveQuery = @"
        UPDATE LeaveRequests
        SET IsApproved = 1
        WHERE LeaveRequestId = @LeaveRequestId";

                using (var command = new SqliteCommand(approveQuery, connection))
                {
                    command.Parameters.AddWithValue("@LeaveRequestId", leaveRequestId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void RejectLeave(int leaveRequestId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                string rejectQuery = @"
        UPDATE LeaveRequests
        SET IsApproved = 0
        WHERE LeaveRequestId = @LeaveRequestId";

                using (var command = new SqliteCommand(rejectQuery, connection))
                {
                    command.Parameters.AddWithValue("@LeaveRequestId", leaveRequestId);
                    command.ExecuteNonQuery();
                }
            }
        }

    }
}