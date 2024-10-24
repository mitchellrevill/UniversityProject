using UniversityProject.Model;

namespace UniversityProject.Interfaces
{
    public interface IJobPostingsService
    {
        Task<IEnumerable<Employee>> GetAllJobPostingsAsync();
        Task InsertJobPostingsAsync(JobPostings JobPostings);
        Task UpdateJobPostingsAsync(JobPostings JobPostings);
        Task DeleteJobPostingsAsync(string postingId);

    }

}