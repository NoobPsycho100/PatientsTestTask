using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PatientsTestTask.Core.Services;
using PatientsTestTask.Data.Context;
using PatientsTestTask.Data.Services;
using PatientsTestTask.Web.Model;
using PatientsTestTask.Web.Validation;

namespace PatientsTestTask.Web;

public static class DI
{
    public static void RegisterCustomDependencies(this IServiceCollection services, ConfigurationManager config)
    {
        services.AddDbContextFactory<PatientsContext>(options => 
        {
            options.UseSqlServer(config.GetConnectionString("PatientsDB"));
        });

        services.AddScoped<IPatientsService, PatientsService>();

        services.RegisterValidators();
    }

    private static void RegisterValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<GetPatientsRequest>, GetPatientsValidator>();
        services.AddScoped<IValidator<AddPatientRequest>, AddPatientValidator>();
        services.AddScoped<IValidator<UpdatePatientRequest>, UpdatePatientValidator>();
    }
}

