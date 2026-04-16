namespace PatientsTestTask.Data.Context.Entities;

internal class PatientName
{
    public required Guid Id { get; set; }

    public string? Use { get; set; }

    public required string Family { get; set; }
}
