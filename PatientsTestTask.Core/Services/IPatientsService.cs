using PatientsTestTask.Core.Domain;

namespace PatientsTestTask.Core.Services;

public interface IPatientsService
{
    public Task<bool> IsPatientExists(Guid id);

    public Task<PageResult<Patient>> GetPatients(string[] birthDateFilters, int page, int pageSize);

    public Task<Patient?> GetPatientById(Guid id);

    public Task<Patient> AddPatient(Patient patient);

    public Task<Patient?> UpdatePatient(Guid id, Patient patient);

    public Task<bool> DeletePatient(Guid id);
}
