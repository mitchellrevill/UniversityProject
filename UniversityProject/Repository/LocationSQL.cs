using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class LocationSQL
    {
        private readonly string _connectionString;

        public LocationSQL(string dbPath)
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
                CREATE TABLE IF NOT EXISTS Locations (
                    LocationId TEXT PRIMARY KEY,
                    RegionId INTEGER NOT NULL,
                    CountryId INTEGER NOT NULL,
                    Latitude REAL NOT NULL,
                    Longitude REAL NOT NULL,
                    LocationName TEXT NOT NULL,
                    FOREIGN KEY (RegionId) REFERENCES Regions(RegionId) ON DELETE CASCADE,
                    FOREIGN KEY (CountryId) REFERENCES Country(CountryId) ON DELETE CASCADE
                );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertLocation(Location location)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                INSERT INTO Locations (LocationId, RegionId, CountryId, Latitude, Longitude, LocationName)
                VALUES (@LocationId, @RegionId, @CountryId, @Latitude, @Longitude, @LocationName)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@LocationId", location.LocationId);
                    command.Parameters.AddWithValue("@RegionId", location.RegionId);
                    command.Parameters.AddWithValue("@CountryId", location.CountryId);
                    command.Parameters.AddWithValue("@Latitude", location.Latitude);
                    command.Parameters.AddWithValue("@Longitude", location.Longitude);
                    command.Parameters.AddWithValue("@LocationName", location.LocationName);

                    command.ExecuteNonQuery();
                }
            }
        }


        public List<Location> GetAllLocations()
        {
            var locations = new List<Location>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Locations";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var location = new Location
                        {
                            LocationId = reader.GetString(0),
                            RegionId = reader.GetInt32(1),
                            CountryId = reader.GetInt32(2),
                            Latitude = reader.GetDouble(3),
                            Longitude = reader.GetDouble(4),
                            LocationName = reader.GetString(5)
                        };

                        locations.Add(location);
                    }
                }
            }

            return locations;
        }

        public Location GetLocationById(string locationId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Locations WHERE LocationId = @LocationId";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@LocationId", locationId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Location
                            {
                                LocationId = reader.GetString(0),
                                RegionId = reader.GetInt32(1),
                                CountryId = reader.GetInt32(2),
                                Latitude = reader.GetDouble(3),
                                Longitude = reader.GetDouble(4),
                                LocationName = reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return null;
        }

        public void UpdateLocation(Location location)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                UPDATE Locations
                SET RegionId = @RegionId,
                    CountryId = @CountryId,
                    Latitude = @Latitude,
                    Longitude = @Longitude,
                    LocationName = @LocationName
                WHERE LocationId = @LocationId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@LocationId", location.LocationId);
                    command.Parameters.AddWithValue("@RegionId", location.RegionId);
                    command.Parameters.AddWithValue("@CountryId", location.CountryId);
                    command.Parameters.AddWithValue("@Latitude", location.Latitude);
                    command.Parameters.AddWithValue("@Longitude", location.Longitude);
                    command.Parameters.AddWithValue("@LocationName", location.LocationName);

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteLocation(string locationId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Locations WHERE LocationId = @LocationId";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@LocationId", locationId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
