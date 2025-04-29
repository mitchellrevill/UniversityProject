using UniversityProject.Interfaces;
using UniversityProject.Model;
using UniversityProject.Repository;

public class ApplicantService : IApplicantService
{
    // Singleton instance
    private static ApplicantService _instance;
    private static readonly object _lock = new object();

    private readonly ApplicationSQL _applicationDatabase;

    private ApplicantService(string dbPath)
    {
        _applicationDatabase = new ApplicationSQL(dbPath);
    }

    // Public method to provide the single instance
    public static ApplicantService GetInstance(string dbPath)
    {

        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new ApplicantService(dbPath);
                }
            }
        }
        return _instance;
    }

    public async Task<IEnumerable<Applicant>> GetAllApplicantsAsync()
    {
        return await Task.Run(() => _applicationDatabase.GetAllApplicants());
    }

    public async Task InsertApplicantAsync(Applicant applicant)
    {
        await Task.Run(() => _applicationDatabase.AddApplicant(applicant));
    }

    public async Task UpdateApplicantAsync(Applicant applicant)
    {
        await Task.Run(() => _applicationDatabase.UpdateApplicant(applicant));
    }

    public async Task DeleteApplicantAsync(string applicantId)
    {
        await Task.Run(() => _applicationDatabase.DeleteApplicant(applicantId));
    }
}
