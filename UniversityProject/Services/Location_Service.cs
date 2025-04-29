using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class LocationService : ILocationService
{
    // Singleton instance
    private static LocationService _instance;


    private static readonly object _lock = new object();


    private readonly LocationSQL _locationDatabase;

  
    private LocationService(string dbPath)
    {
        _locationDatabase = new LocationSQL(dbPath);
    }

  
    public static LocationService GetInstance(string dbPath)
    {
        if (_instance == null)
        {
            lock (_lock) // Ensure thread safety
            {
                if (_instance == null)
                {
                    _instance = new LocationService(dbPath);
                }
            }
        }
        return _instance;
    }


    public async Task<IEnumerable<Location>> GetAllLocationsAsync()
    {
        return await Task.Run(() => _locationDatabase.GetAllLocations());
    }

    public async Task<Location> GetLocationByIdAsync(string locationId)
    {
        return await Task.Run(() => _locationDatabase.GetLocationById(locationId));
    }

    public async Task InsertLocationAsync(Location location)
    {
        await Task.Run(() => _locationDatabase.InsertLocation(location));
    }

    public async Task UpdateLocationAsync(Location location)
    {
        await Task.Run(() => _locationDatabase.UpdateLocation(location));
    }

    public async Task DeleteLocationAsync(string locationId)
    {
        await Task.Run(() => _locationDatabase.DeleteLocation(locationId));
    }

    public async Task<Location> GetLocationIdAsync(string locationId)
    {
        return await Task.Run(() => _locationDatabase.GetLocationById(locationId));
    }
}
