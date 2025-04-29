using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class RegionService : IRegionService
{
    // Singleton instance
    private static RegionService _instance;


    private static readonly object _lock = new object();

    private readonly RegionSQL _regionDatabase;


    private RegionService(string dbPath)
    {
        _regionDatabase = new RegionSQL(dbPath);
    }

    public static RegionService GetInstance(string dbPath)
    {
        if (_instance == null)
        {
            lock (_lock) 
            {
                if (_instance == null)
                {
                    _instance = new RegionService(dbPath);
                }
            }
        }
        return _instance;
    }

    public async Task<IEnumerable<Region>> GetAllRegionsAsync()
    {
        return await Task.Run(() => _regionDatabase.GetAllRegions());
    }

    public async Task InsertRegionAsync(Region region)
    {
        await Task.Run(() => _regionDatabase.InsertRegion(region));
    }

    public async Task UpdateRegionAsync(Region region)
    {
        await Task.Run(() => _regionDatabase.UpdateRegion(region));
    }

    public async Task DeleteRegionAsync(int regionId)
    {
        await Task.Run(() => _regionDatabase.DeleteRegion(regionId));
    }
}
