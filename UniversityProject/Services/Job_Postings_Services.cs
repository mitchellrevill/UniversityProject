using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class JobPostingsService : IJobPostingsService
{
    // Singleton instance
    private static JobPostingsService _instance;


    private static readonly object _lock = new object();


    private readonly JobPostingSQL _RecruitmentDatabase;


    private JobPostingsService(string dbPath)
    {
        _RecruitmentDatabase = new JobPostingSQL(dbPath);
    }


    public static JobPostingsService GetInstance(string dbPath)
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new JobPostingsService(dbPath);
                }
            }
        }
        return _instance;
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
