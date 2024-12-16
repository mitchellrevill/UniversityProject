using Microsoft.Data.Sqlite;
using UniversityProject.Model;

namespace UniversityProject.Repository
{
    public class CountrySQL
    {
        private readonly string _connectionString;

        public CountrySQL(string dbPath)
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
                CREATE TABLE IF NOT EXISTS Country (
                    CountryId INTEGER PRIMARY KEY AUTOINCREMENT,
                    CountryName TEXT NOT NULL,
                    CountryCurrency TEXT NOT NULL,
                    LegalRequirements TEXT NOT NULL,
                    MinimumLeave INTEGER NOT NULL
                )";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }


        public void InsertCountry(Country country)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
        INSERT INTO Country (CountryName, CountryCurrency, LegalRequirements, MinimumLeave)
        VALUES (@CountryName, @CountryCurrency, @LegalRequirements, @MinimumLeave)";


                Console.WriteLine("Inserting Country:");
                Console.WriteLine($"CountryName: {country.CountryName}");
                Console.WriteLine($"CountryCurrency: {country.CountryCurrency}");
                Console.WriteLine($"LegalRequirements: {string.Join(",", country.LegalRequirements)}");
                Console.WriteLine($"MinimumLeave: {country.MinimumLeave}");
                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@CountryName", country.CountryName);
                    command.Parameters.AddWithValue("@CountryCurrency", country.CountryCurrency);
                    command.Parameters.AddWithValue("@LegalRequirements", string.Join(",", country.LegalRequirements));
                    command.Parameters.AddWithValue("@MinimumLeave", country.MinimumLeave);

                    command.ExecuteNonQuery();
                }
            }
        }



        public List<Country> GetAllCountries()
        {
            var countries = new List<Country>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Country";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var country = new Country
                        {
                            CountryId = reader.GetString(0),
                            CountryName = reader.GetString(1),
                            CountryCurrency = reader.GetString(2),
                            LegalRequirements = new List<string>(reader.GetString(3).Split(',')),
                            MinimumLeave = reader.GetInt32(4)
                        };

                        countries.Add(country);
                    }
                }
            }

            return countries;
        }

        public Country GetCountryById(string countryId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM Country WHERE CountryId = @CountryId";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@CountryId", countryId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Country
                            {
                                CountryId = reader.GetString(0),
                                CountryName = reader.GetString(1),
                                CountryCurrency = reader.GetString(2),
                                LegalRequirements = new List<string>(reader.GetString(3).Split(',')),
                                MinimumLeave = reader.GetInt32(4)
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Update a country
        public void UpdateCountry(Country country)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                Console.WriteLine("Inserting Country:");
                Console.WriteLine($"CountryName: {country.CountryName}");
                Console.WriteLine($"CountryCurrency: {country.CountryCurrency}");
                Console.WriteLine($"LegalRequirements: {string.Join(",", country.LegalRequirements)}");
                Console.WriteLine($"MinimumLeave: {country.MinimumLeave}");

                string updateQuery = @"
                UPDATE Country
                SET CountryName = @CountryName,
                    CountryCurrency = @CountryCurrency,
                    LegalRequirements = @LegalRequirements,
                    MinimumLeave = @MinimumLeave
                WHERE CountryId = @CountryId";

                using (var command = new SqliteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@CountryId", country.CountryId);
                    command.Parameters.AddWithValue("@CountryName", country.CountryName);
                    command.Parameters.AddWithValue("@CountryCurrency", country.CountryCurrency);
                    command.Parameters.AddWithValue("@LegalRequirements", string.Join(",", country.LegalRequirements));
                    command.Parameters.AddWithValue("@MinimumLeave", country.MinimumLeave);

                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete a country by its ID
        public void DeleteCountry(string countryId)
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM Country WHERE CountryId = @CountryId";
                using (var command = new SqliteCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
