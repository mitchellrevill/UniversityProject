using UniversityProject.Model;
using UniversityProject.Repository;
using UniversityProject.Interfaces;

public class JobPostingsService : IJobPostingsService
{
	private readonly RecruitmentSQL _RecruitmentDatabase;

	public JobPostingsService(string dbPath)
	{
		_RecruitmentDatabase = new RecruitmentSQL(dbPath);
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

	public async Task DeleteJobPostingsAsync(string postingId)
	{
		await Task.Run(() => _RecruitmentDatabase.DeleteJob(postingId));
	}
}
