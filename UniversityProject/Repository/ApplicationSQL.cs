using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class ApplicationSQL
    {

        private readonly string _connectionString;

        public ApplicationSQL(string dbPath)
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
                    CREATE TABLE IF NOT EXISTS Applicant (
                    ApplicantId TEXT PRIMARY KEY,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL,
                    CoverLetter TEXT,
                    Gender TEXT,
                    City TEXT,
                    Region TEXT,
                    Country TEXT,
                    Phone TEXT,
                    CVpdfContent TEXT,
                    CVfileName TEXT
                    )";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
        public void AddApplicant(Applicant applicant)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                INSERT INTO Applicant (ApplicantId, FirstName, LastName, CoverLetter, Gender, City, Region, Country, Phone, CVpdfContent, CVfileName)
                VALUES (@ApplicantId, @FirstName, @LastName, @CoverLetter, @Gender, @City, @Region, @Country, @Phone, @CVpdfContent, @CVfileName)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantId", applicant.applicantId);
                    command.Parameters.AddWithValue("@FirstName", applicant.firstName);
                    command.Parameters.AddWithValue("@LastName", applicant.lastName);
                    command.Parameters.AddWithValue("@CoverLetter", applicant.coverletter);
                    command.Parameters.AddWithValue("@Gender", applicant.Gender);
                    command.Parameters.AddWithValue("@City", applicant.City);
                    command.Parameters.AddWithValue("@Region", applicant.Region);
                    command.Parameters.AddWithValue("@Country", applicant.Country);
                    command.Parameters.AddWithValue("@Phone", applicant.Phone);
                    command.Parameters.AddWithValue("@CVpdfContent", applicant.CVpdfContent);
                    command.Parameters.AddWithValue("@CVfileName", applicant.CVfileName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Applicant> GetAllApplicants()
        {
            var applicants = new List<Applicant>();
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Applicant";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var applicant = new Applicant
                        {
                            applicantId = reader.GetString(0),
                            firstName = reader.GetString(1),
                            lastName = reader.GetString(2),
                            coverletter = reader.GetString(3),
                            Gender = reader.GetString(4),
                            City = reader.GetString(5),
                            Region = reader.GetString(6),
                            Country = reader.GetString(7),
                            Phone = reader.GetString(8),
                            CVpdfContent = reader.GetString(9),
                            CVfileName = reader.GetString(10)
                        };
                        applicants.Add(applicant);
                    }
                }
            }
            return applicants;
        }

        public void UpdateApplicant(Applicant applicant)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE Applicants
                SET firstName = @firstName,
                    lastName = @lastName,
                    coverletter = @coverletter,
                    Gender = @Gender,
                    City = @City,
                    Region = @Region,
                    Country = @Country,
                    Phone = @Phone,
                    CVpdfContent = @CVpdfContent,
                    CVfileName = @CVfileName
                WHERE applicantId = @applicantId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@applicantId", applicant.applicantId);
                    command.Parameters.AddWithValue("@firstName", applicant.firstName);
                    command.Parameters.AddWithValue("@lastName", applicant.lastName);
                    command.Parameters.AddWithValue("@coverletter", applicant.coverletter);
                    command.Parameters.AddWithValue("@Gender", applicant.Gender);
                    command.Parameters.AddWithValue("@City", applicant.City);
                    command.Parameters.AddWithValue("@Region", applicant.Region);
                    command.Parameters.AddWithValue("@Country", applicant.Country);
                    command.Parameters.AddWithValue("@Phone", applicant.Phone);
                    command.Parameters.AddWithValue("@CVpdfContent", applicant.CVpdfContent);
                    command.Parameters.AddWithValue("@CVfileName", applicant.CVfileName);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteApplicant(string applicantId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Applicant WHERE ApplicantId = @ApplicantId";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantId", applicantId);
                    command.ExecuteNonQuery();
                }
            }
        }

    }

}

