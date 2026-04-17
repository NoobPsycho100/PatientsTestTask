using PatientsTestTask.Web;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ErrorsFilter>();
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddOpenApiAndSwagger();
builder.Services.RegisterCustomDependencies(builder.Configuration);

var app = builder.Build();

app.UseOpenApiAndSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
