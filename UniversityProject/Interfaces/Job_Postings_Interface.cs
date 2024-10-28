using UniversityProject.Model;

namespace UniversityProject.Interfaces
{
    public interface IJobPostingsService
    {
        Task<IEnumerable<JobPostings>> GetAllJobPostingsAsync();
        Task InsertJobPostingsAsync(JobPostings JobPostings);
        Task UpdateJobPostingsAsync(JobPostings JobPostings);
        Task DeleteJobPostingsAsync(string postingId);
        Task GetJobPostingByIdAsync(string postingId);

    }

}