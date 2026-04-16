namespace PatientsTestTask.Core.Domain;

public class Patient
{
    public required PatientName Name { get; set; }

    public Gender? Gender { get; set; }

    public required DateTime BirthDate { get; set; }

    public bool? IsActive { get; set; }


    public class PatientName
    {
        public Guid? Id { get; set; }

        public string? Use { get; set; }

        public required string Family { get; set; }

        public string[]? Given { get; set; }
    }
}