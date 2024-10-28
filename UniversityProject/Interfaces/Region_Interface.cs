using UniversityProject.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversityProject.Interfaces
{
    public interface IRegionService
    {
        Task<IEnumerable<Region>> GetAllRegionsAsync();
        Task InsertRegionAsync(Region region);
        Task UpdateRegionAsync(Region region);
        Task DeleteRegionAsync(string regionId);
    }
}
