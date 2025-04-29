using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class CountryService : ICountryService
{
    private static CountryService _instance;
    private static readonly object _lock = new object();

    private readonly CountrySQL _CountrySQL;

    // Private constructor to prevent external instantiation
    private CountryService(string dbPath)
    {
        _CountrySQL = new CountrySQL(dbPath);
    }

    // Public static method to get the single instance of CountryService
    public static CountryService GetInstance(string dbPath)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new CountryService(dbPath);
                }
            }
        }
        return _instance;
    }

    public async Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        return await Task.Run(() => _CountrySQL.GetAllCountries());
    }

    public async Task GetCountryByIdAsync(string CountryId)
    {
        await Task.Run(() => _CountrySQL.GetCountryById(CountryId));
    }

    public async Task InsertCountryAsync(Country country)
    {
        await Task.Run(() => _CountrySQL.InsertCountry(country));
    }

    public async Task UpdateCountryAsync(Country country)
    {
        await Task.Run(() => _CountrySQL.UpdateCountry(country));
    }

    public async Task DeleteCountryAsync(string CountryId)
    {
        await Task.Run(() => _CountrySQL.DeleteCountry(CountryId));
    }
}
