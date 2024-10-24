using System;
using System.Collections.Generic;

namespace UniversityProject.Model
{
    public class Country
    {
        public string CountryId { get; set; }

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
