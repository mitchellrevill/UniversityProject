using Microsoft.Data.Sqlite;
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
                    FOREIGN KEY (CountryId) REFERENCES Country(CountryId) ON DELETE CASCADE
                );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertRegion(Region region)
        {
            try
            {
                using (var connection = new SqliteConnection(_connectionString))
                {
                    connection.Open();
                    Console.WriteLine(region.RegionName);
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
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
        }

        public List<Region> GetAllRegions()
        {
            var regions = new List<Region>();
            try
            {
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
                                RegionId = reader.GetInt32(0),
                                RegionName = reader.GetString(1),
                                CountryId = reader.GetString(2)
                            };

                            regions.Add(region);
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return regions;
        }

        public Region GetRegionById(int regionId)
        {
            try
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
                                    RegionId = reader.GetInt32(0),
                                    RegionName = reader.GetString(1),
                                    CountryId = reader.GetString(2)
                                };
                            }
                        }
                    }
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return null;
        }

        public void UpdateRegion(Region region)
        {
            try
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
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
        }

        public void DeleteRegion(int regionId)
        {
            try
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
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");

            }
        }
    }
}
