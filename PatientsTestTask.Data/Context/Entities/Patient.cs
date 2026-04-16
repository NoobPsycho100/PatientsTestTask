namespace PatientsTestTask.Data.Context.Entities;

internal class Patient
{
    public required Guid Id { get; set; }

    public string? Gender { get; set; }

    public required DateTime BirthDate { get; set; }

    public bool? IsActive { get; set; }
}
