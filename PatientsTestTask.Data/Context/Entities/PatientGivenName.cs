namespace PatientsTestTask.Data.Context.Entities;

internal class PatientGivenName
{
    public required Guid PatientId { get; set; }

    public required byte Position { get; set; }

    public required string Name { get; set; }
}
