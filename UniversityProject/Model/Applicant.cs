using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityProject.Model
{

    //FIRSTNAME LASTNAME GENDER COVERLETTER CVPDFCONTENT CVDPFNAME


    public class Applicant
    {
        public string applicantId { get; set; }
        public string firstName { get; set; }

        public string postingId { get; set; }
        public string coverletter { get; set; }
        public string lastName { get; set; }
        public string Gender { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string CVpdfContent { get; set; }
        public string CVfileName { get; set; }

    }
}
