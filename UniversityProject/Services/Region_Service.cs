using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class RegionService : IRegionService
{
    private readonly RegionSQL _regionDatabase;

    public RegionService(string dbPath)
    {
        _regionDatabase = new RegionSQL(dbPath);
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
