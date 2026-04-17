using PatientsTestTask.Core.Domain;

namespace PatientsTestTask.Web.Model;

public class AddPatientRequest
{
    public required AddPatientNameRequest Name { get; set; }

    public Gender? Gender { get; set; }

    public required DateTime BirthDate { get; set; }

    public bool? IsActive { get; set; }


    public class AddPatientNameRequest
    {
        public Guid? Id { get; set; }

        public string? Use { get; set; }

        public required string Family { get; set; }

        public string[]? Given { get; set; }
    }
}
