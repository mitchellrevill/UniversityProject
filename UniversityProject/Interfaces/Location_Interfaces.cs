using UniversityProject.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversityProject.Interfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAllLocationsAsync();
        Task<Location> GetLocationByIdAsync(string locationId);
        Task InsertLocationAsync(Location location);
        Task UpdateLocationAsync(Location location);
        Task DeleteLocationAsync(string locationId);
    }
}
