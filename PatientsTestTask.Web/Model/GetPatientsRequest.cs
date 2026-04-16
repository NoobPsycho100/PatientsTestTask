namespace PatientsTestTask.Web.Model;

public class GetPatientsRequest: PagedRequest
{
    public DateTime? BirthDateFrom { get; set; }
    public DateTime? BirthDateTo { get; set; }
}
