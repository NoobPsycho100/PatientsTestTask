using Microsoft.AspNetCore.Mvc;
using PatientsTestTask.Core.Domain;
using PatientsTestTask.Core.Services;

namespace PatientsTestTask.Web.Controllers;

[ApiController]
[Route("patients")]
public class PatientsController : ControllerBase
{
    private readonly IPatientsService _patientsService;

    public PatientsController(IPatientsService patientsService)
    {
        _patientsService = patientsService;
    }

    [Route("{id}")]
    [HttpGet]
    public async Task<Patient?> GetById([FromRoute] string id)
    {
        return await _patientsService.GetPatientById(Guid.Parse(id));
    }
}
