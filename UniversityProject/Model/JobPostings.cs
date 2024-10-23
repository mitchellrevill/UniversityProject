using System;

namespace UniversityProject.Model
{
    public class JobPostings
    {
        public string postingId { get; set; }
        public string Title { get; set; }
        public string Salary { get; set; }
        public string JobDescription { get; set; }
        public string JobType { get; set; }

        public string Hours { get; set; }
    }
}
