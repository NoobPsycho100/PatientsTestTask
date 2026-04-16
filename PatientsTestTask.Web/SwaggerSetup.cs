using Microsoft.OpenApi;

namespace PatientsTestTask.Web;

public static class SwaggerSetup
{
    public static void AddOpenApiAndSwagger(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Patients test task api", Version = "v1" });
            options.SupportNonNullableReferenceTypes();
        });
    }

    public static void UseOpenApiAndSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // https://localhost:7165/openapi/v1.json
            app.MapOpenApi();

            // https://localhost:7165/swagger/index.html
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
