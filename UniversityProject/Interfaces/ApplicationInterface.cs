using UniversityProject.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UniversityProject.Interfaces
{
    public interface IApplicantService
    {
        Task<IEnumerable<Applicant>> GetAllApplicantsAsync();
        Task InsertApplicantAsync(Applicant applicant);
        Task UpdateApplicantAsync(Applicant applicant);
        Task DeleteApplicantAsync(string applicantId);
    }
}
