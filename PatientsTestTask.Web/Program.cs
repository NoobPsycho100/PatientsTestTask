using PatientsTestTask.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApiAndSwagger();
builder.Services.RegisterCustomDependencies(builder.Configuration);

var app = builder.Build();

app.UseOpenApiAndSwagger();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
