namespace UniversityProject.Model
{
    public class Location
    {
        public string LocationId { get; set; }
        public int RegionId { get; set; }
        public int CountryId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string LocationName { get; set; }
    }
}
