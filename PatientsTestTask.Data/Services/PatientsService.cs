using Microsoft.EntityFrameworkCore;
using PatientsTestTask.Core;
using PatientsTestTask.Core.Domain;
using PatientsTestTask.Core.Services;
using PatientsTestTask.Data.Context;
using PatientsTestTask.Data.Context.Entities;
using Patient = PatientsTestTask.Core.Domain.Patient;

namespace PatientsTestTask.Data.Services;

public class PatientsService: IPatientsService
{
    private readonly IDbContextFactory<PatientsContext> _contextFactory;

    public PatientsService(IDbContextFactory<PatientsContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<bool> IsPatientExists(Guid id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await GetPatientsQuery(context)
                .AnyAsync(x => x.Name.Id == id);
        }
    }

    public async Task<PageResult<Patient>> GetPatients(string[] birthDateFilters, int page, int pageSize = 100)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var query = GetPatientsQuery(context);
            foreach (var filter in birthDateFilters)
            {
                query = query.Where(filter.ToDateFilterExpression<Patient>(x => x.BirthDate));
            }

            return await query.OrderBy(x => x.Name.Id).ToPageResult(page, pageSize);
        }
    }

    public async Task<Patient?> GetPatientById(Guid id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await GetPatientsQuery(context)
                .SingleOrDefaultAsync(x => x.Name.Id == id);
        }
    }

    public async Task<Patient> AddPatient(Patient patient)
    {
        var id = patient.Name.Id ?? Guid.NewGuid();
        var patientDto = new Context.Entities.Patient
        {
            Id = id,
            BirthDate = patient.BirthDate,
            Gender = patient.Gender?.ToString(),
            IsActive = patient.IsActive,
        };
        var patientNameDto = new Context.Entities.PatientName
        {
            Id = id,
            Family = patient.Name.Family,
            Use = patient.Name.Use,
        };
        var patientGivenNames = patient.Name.Given
            ?.Select((x, i) => new Context.Entities.PatientGivenName
            {
                PatientId = id,
                Name = x,
                Position = (byte)i,
            }).ToArray() ?? [];

        using (var context = _contextFactory.CreateDbContext())
        {
            context.AddRange(patientGivenNames);
            context.Add(patientNameDto);
            context.Add(patientDto);
            await context.SaveChangesAsync();

            return await GetPatientsQuery(context)
                .SingleAsync(x => x.Name.Id == id);
        }
    }

    public async Task<Patient?> UpdatePatient(Guid id, Patient patient)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var patientDto = await context.Patients
                .Include(x => x.Name)
                .Include(x => x.Name.GivenNames)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();

            if (patientDto == null)
            {
                return null;
            }

            patientDto.Gender = patient.Gender?.ToString();
            patientDto.BirthDate = patient.BirthDate;
            patientDto.IsActive = patient.IsActive;
            patientDto.Name.Use = patient.Name.Use;
            patientDto.Name.Family = patient.Name.Family;
            patientDto.Name.GivenNames = patient.Name.Given
                ?.Select((x, i) => new PatientGivenName
                {
                    PatientId = id,
                    Name = x,
                    Position = (byte)i,
                }).ToList() ?? [];

            await context.SaveChangesAsync();

            return await GetPatientsQuery(context)
                .SingleAsync(x => x.Name.Id == id);
        }
    }

    public async Task<bool> DeletePatient(Guid id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var deletedRows = await context.Patients.Where(x => x.Id == id).ExecuteDeleteAsync();

            return deletedRows > 0;
        }
    }

    private static IQueryable<Patient> GetPatientsQuery(PatientsContext context)
    {
        return context.Patients
            .Include(x => x.Name)
            .Include(x => x.Name.GivenNames)
            .Select(x => new Patient
            {
                Name = new Patient.PatientName
                {
                    Id = x.Name.Id,
                    Family = x.Name.Family,
                    Use = x.Name.Use,
                    Given = x.Name.GivenNames.OrderBy(g => g.Position).Select(g => g.Name).ToArray()
                },
                Gender = x.Gender != null ? Enum.Parse<Gender>(x.Gender) : null,
                BirthDate = x.BirthDate,
                IsActive = x.IsActive,
            });
    }
}
