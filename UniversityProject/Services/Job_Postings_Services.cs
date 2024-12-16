using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class JobPostingsService : IJobPostingsService
{
    private readonly JobPostingSQL _RecruitmentDatabase;

    public JobPostingsService(string dbPath)
    {
        _RecruitmentDatabase = new JobPostingSQL(dbPath);
    }

    public async Task<IEnumerable<JobPostings>> GetAllJobPostingsAsync()
    {
        return await Task.Run(() => _RecruitmentDatabase.GetAllJobPostings());
    }

    public async Task InsertJobPostingsAsync(JobPostings JobPostings)
    {
        await Task.Run(() => _RecruitmentDatabase.AddJobPost(JobPostings));
    }

    public async Task UpdateJobPostingsAsync(JobPostings JobPostings)
    {
        await Task.Run(() => _RecruitmentDatabase.UpdateJobPosting(JobPostings));
    }
    public async Task<JobPostings> GetJobPostingByIdAsync(string postingId)
    {
        return await Task.Run(() => _RecruitmentDatabase.GetJobPostingById(postingId));
    }
    public async Task DeleteJobPostingsAsync(string postingId)
    {
        await Task.Run(() => _RecruitmentDatabase.DeleteJob(postingId));
    }
}
