using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class RegionSQL
    {
        private readonly string _connectionString;

        public RegionSQL(string dbPath)
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
                CREATE TABLE IF NOT EXISTS Regions (
                    RegionId INTEGER PRIMARY KEY AUTOINCREMENT,
                    RegionName TEXT NOT NULL,
                    CountryId INTEGER NOT NULL,
                    FOREIGN KEY (CountryId) REFERENCES Countries(CountryId) ON DELETE CASCADE
                )";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertRegion(Region region)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                INSERT INTO Regions (RegionName, CountryId)
                VALUES (@RegionName, @CountryId)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@RegionName", region.RegionName);
                    command.Parameters.AddWithValue("@CountryId", region.CountryId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Region> GetAllRegions()
        {
            var regions = new List<Region>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Regions";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var region = new Region
                        {
                            RegionId = reader.GetString(0),
                            RegionName = reader.GetString(1),
                            CountryId = reader.GetString(2)
                        };

                        regions.Add(region);
                    }
                }
            }

            return regions;
        }

        public Region GetRegionById(int regionId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Regions WHERE RegionId = @RegionId";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@RegionId", regionId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Region
                            {
                                RegionId = reader.GetString(0),
                                RegionName = reader.GetString(1),
                                CountryId = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateRegion(Region region)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE Regions
                SET RegionName = @RegionName,
                    CountryId = @CountryId
                WHERE RegionId = @RegionId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@RegionId", region.RegionId);
                    command.Parameters.AddWithValue("@RegionName", region.RegionName);
                    command.Parameters.AddWithValue("@CountryId", region.CountryId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRegion(string regionId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Regions WHERE RegionId = @RegionId";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@RegionId", regionId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
