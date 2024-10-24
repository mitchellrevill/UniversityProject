using System;

namespace UniversityProject.Model
{
    public class Location
    {
        public string LocationId { get; set; } 
        public string RegionId { get; set; }   
        public string CountryId { get; set; }  
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LocationName { get; set; }
    }
}
