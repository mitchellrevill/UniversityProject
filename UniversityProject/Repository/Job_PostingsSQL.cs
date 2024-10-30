using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class JobPostingSQL
    {
        private readonly string _connectionString;

        public JobPostingSQL(string dbPath)
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
                    salary TEXT NOT NULL,
                    LocationId TEXT NOT NULL,
                    FOREIGN KEY (LocationId) REFERENCES Locations(LocationId)
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
                INSERT INTO JobPostings (postingId, Title, jobdesc, jobtype, hours, salary, LocationId)
                VALUES (@postingId, @Title, @jobdesc, @jobtype, @hours, @salary, @LocationId)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@postingId", Post.postingId);
                    command.Parameters.AddWithValue("@Title", Post.Title);
                    command.Parameters.AddWithValue("@jobdesc", Post.JobDescription);
                    command.Parameters.AddWithValue("@jobtype", Post.JobType);
                    command.Parameters.AddWithValue("@hours", Post.Hours);
                    command.Parameters.AddWithValue("@salary", Post.Salary);
                    command.Parameters.AddWithValue("@LocationId", Post.LocationId); 
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<JobPostings> GetAllJobPostings()
        {
            var jobs = new List<JobPostings>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM JobPostings";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var job = new JobPostings
                        {
                            postingId = reader.GetString(0),
                            Title = reader.GetString(1),
                            JobDescription = reader.GetString(2),
                            JobType = reader.GetString(3),
                            Hours = reader.GetString(4),
                            Salary = reader.GetString(5),
                            LocationId = reader.GetString(6) 
                        };
                        jobs.Add(job);
                    }
                }
            }
            return jobs;
        }

        public void DeleteJob(string postingId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM JobPostings WHERE postingId = @postingId";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@postingId", postingId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public JobPostings GetJobPostingById(string postingId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM JobPostings WHERE postingId = @PostingId";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@PostingId", postingId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new JobPostings
                            {
                                postingId = reader.GetString(0),
                                Title = reader.GetString(1),
                                JobDescription = reader.GetString(2),
                                JobType = reader.GetString(3),
                                Hours = reader.GetString(4),
                                Salary = reader.GetString(5),
                                LocationId = reader.GetString(6) 
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void UpdateJobPosting(JobPostings Post)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE JobPostings
                SET Title = @Title,
                    Salary = @Salary,
                    JobDescription = @JobDescription,
                    JobType = @JobType,
                    Hours = @Hours,
                    LocationId = @LocationId
                WHERE postingId = @postingId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@postingId", Post.postingId);
                    command.Parameters.AddWithValue("@Title", Post.Title);
                    command.Parameters.AddWithValue("@jobdesc", Post.JobDescription);
                    command.Parameters.AddWithValue("@jobtype", Post.JobType);
                    command.Parameters.AddWithValue("@hours", Post.Hours);
                    command.Parameters.AddWithValue("@salary", Post.Salary);
                    command.Parameters.AddWithValue("@LocationId", Post.LocationId); // New field
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
