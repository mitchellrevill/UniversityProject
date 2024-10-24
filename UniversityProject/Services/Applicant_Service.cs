using UniversityProject.Model;
using UniversityProject.Repository;
using UniversityProject.Interfaces;

public class Applicant_Service : IApplicantService
{
    private readonly ApplicationSQL _applicationDatabase;

    public Applicant_Service(string dbPath)
    {
        _applicationDatabase = new ApplicationSQL(dbPath);
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
