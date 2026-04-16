using Microsoft.AspNetCore.Mvc;
using PatientsTestTask.Core;
using PatientsTestTask.Core.Domain;
using PatientsTestTask.Core.Services;
using PatientsTestTask.Web.Model;

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

    [HttpGet]
    public async Task<PageResult<Patient>> GetPaged([FromQuery] GetPatientsRequest request)
    {
        var patient = await _patientsService.GetPatients(request.BirthDateFrom, request.BirthDateTo, request.Page, request.PageSize);
        return patient;
    }

    [Route("{id}")]
    [HttpGet]
    public async Task<ActionResult<Patient>> GetById([FromRoute] string id)
    {
        if (!Guid.TryParse(id, out var patientId))
        {
            return BadRequest();
        }

        var patient = await _patientsService.GetPatientById(patientId);
        if (patient == null)
        {
            return NotFound();
        }
        return patient;
    }
}
