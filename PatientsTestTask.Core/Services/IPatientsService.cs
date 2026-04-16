using PatientsTestTask.Core.Domain;

namespace PatientsTestTask.Core.Services;

public interface IPatientsService
{
    public Task<Patient?> GetPatientById(Guid id);
}
