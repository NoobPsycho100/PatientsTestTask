using Microsoft.EntityFrameworkCore;
using PatientsTestTask.Core.Domain;
using PatientsTestTask.Core.Services;
using PatientsTestTask.Data.Context;

using Patient = PatientsTestTask.Core.Domain.Patient;

namespace PatientsTestTask.Data.Services;

public class PatientsService: IPatientsService
{
    private readonly IDbContextFactory<PatientsContext> _contextFactory;

    public PatientsService(IDbContextFactory<PatientsContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Patient?> GetPatientById(Guid id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await GetPatientsQueue(context)
                .SingleOrDefaultAsync(x => x.Name.Id == id);
        }
    }

    private static IQueryable<Patient> GetPatientsQueue(PatientsContext context)
    {
        return
            from p in context.Patients
            join n in context.PatientNames on p.Id equals n.Id
            select new Patient
            {
                Name = new Patient.PatientName
                {
                    Id = n.Id,
                    Family = n.Family,
                    Use = n.Use,
                    Given = (from g in context.PatientGivenNames
                            where g.PatientId == p.Id
                            orderby g.Position
                            select g.Name).ToArray(),
                },
                Gender = p.Gender != null ? Enum.Parse<Gender>(p.Gender) : null,
                BirthDate = p.BirthDate,
                IsActive = p.IsActive,
            };
    }
}
