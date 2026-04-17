namespace PatientsTestTask.Web.Model;

public class GetPatientsRequest: PagedRequest
{
    public string[] BirthDate { get; set; } = [];
}
