using UniversityProject.Model;

namespace UniversityProject.Interfaces
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task GetCountryByIdAsync(string countryId);
        Task InsertCountryAsync(Country countryvice);
        Task UpdateCountryAsync(Country country);
        Task DeleteCountryAsync(string LocationId);

    }

}