using PatientsTestTask.Core.Domain;

namespace PatientsTestTask.Core.Services;

public interface IPatientsService
{
    public Task<PageResult<Patient>> GetPatients(DateTime? birthDateFrom, DateTime? birthDateTo, int page, int pageSize);

    public Task<Patient?> GetPatientById(Guid id);
}
