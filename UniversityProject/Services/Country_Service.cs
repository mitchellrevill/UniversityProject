using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class CountryService : ICountryService
{
    private readonly CountrySQL _CountrySQL;

    public CountryService(string dbPath)
    {
        _CountrySQL = new CountrySQL(dbPath);
    }

    public async Task<IEnumerable<Country>> GetAllCountriesAsync()
    {
        return await Task.Run(() => _CountrySQL.GetAllCountries());
    }


    public async Task GetCountryByIdAsync(string CountryId)
    {
        await Task.Run(() => _CountrySQL.GetCountryById(CountryId));
    }

    public async Task InsertCountryAsync(Country CountryId)
    {
        await Task.Run(() => _CountrySQL.InsertCountry(CountryId));
    }

    public async Task UpdateCountryAsync(Country CountryId)
    {
        
        await Task.Run(() => _CountrySQL.UpdateCountry(CountryId));
    }

    public async Task DeleteCountryAsync(string CountryId)
    {
        await Task.Run(() => _CountrySQL.DeleteCountry(CountryId));
    }
}
