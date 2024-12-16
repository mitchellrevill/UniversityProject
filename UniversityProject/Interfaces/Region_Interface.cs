using UniversityProject.Model;

namespace UniversityProject.Interfaces
{
    public interface IRegionService
    {
        Task<IEnumerable<Region>> GetAllRegionsAsync();
        Task InsertRegionAsync(Region region);
        Task UpdateRegionAsync(Region region);
        Task DeleteRegionAsync(int regionId);
    }
}
