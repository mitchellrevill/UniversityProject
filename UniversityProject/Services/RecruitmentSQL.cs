using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityProject.Model;
namespace UniversityProject.Services
{
    public class RecruitmentSQL
    {
        private readonly string _connectionString;
        public RecruitmentSQL(string dbPath)
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
                CREATE TABLE IF NOT EXISTS JobPostings (
                    postingId TEXT PRIMARY KEY,
                    Title TEXT NOT NULL,
                    jobdesc TEXT NOT NULL,
                    jobtype TEXT NOT NULL,
                    hours TEXT NOT NULL,
                    salary TEXT NOT NULL
                )";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

            }
        }
        public void AddJobPost(JobPostings Post)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                INSERT INTO JobPostings (postingid, Title, jobdesc, jobtype, hours, )
                VALUES (@postingid, @Title, @jobdesc, @jobtype, @hours,)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@postingId", Post.postingId);
                    command.Parameters.AddWithValue("@Title", Post.Title);
                    command.Parameters.AddWithValue("@jobdesc", Post.JobDescription);
                    command.Parameters.AddWithValue("@jobtype", Post.JobType);
                    command.Parameters.AddWithValue("@hours", Post.Hours);
                    command.Parameters.AddWithValue("@salary", Post.Salary);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void GetAllJobPostings()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM JobPostings";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var employee = new JobPostings
                        {
                            postingId = reader.GetString(0),
                            Title = reader.GetString(1),
                            JobDescription = reader.GetString(2),
                            JobType = reader.GetString(3),
                            Hours = reader.GetString(4),
                            Salary = reader.GetString(5)

                        };
                    }
                }
            }
        }
    }
}
        