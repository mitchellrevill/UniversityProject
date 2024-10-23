using System;
using System.Collections.Generic;

namespace UniversityProject.Model
{
    internal class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string CountryCurrency { get; set; }
        public List<string> LegalRequirements { get; set; }
        public int MinimumLeave { get; set; }

        public Country()
        {
            LegalRequirements = new List<string>();
        }
    }
}
