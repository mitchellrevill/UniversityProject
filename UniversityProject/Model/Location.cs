using System;

namespace UniversityProject.Model
{
    internal class Location
    {
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public int LocationId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LocationName { get; set; }
    }
}
