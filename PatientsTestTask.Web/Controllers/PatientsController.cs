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
    public async Task<ActionResult<Patient>> GetById([FromRoute] Guid id)
    {
        var patient = await _patientsService.GetPatientById(id);
        if (patient == null)
        {
            return NotFound();
        }
        return patient;
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> Add([FromBody] AddPatientRequest request)
    {
        var patient = await _patientsService.AddPatient(Map(request));
        return patient;
    }

    [Route("{id}")]
    [HttpPut]
    public async Task<ActionResult<Patient>> Update([FromRoute] Guid id, UpdatePatientRequest request)
    {
        var patient = await _patientsService.UpdatePatient(id, Map(request));
        if (patient == null)
        {
            return NotFound();
        }
        return patient;
    }

    [Route("{id}")]
    [HttpDelete]
    public async Task<ActionResult> Delete([FromRoute] Guid id)
    {
        var deleted = await _patientsService.DeletePatient(id);
        return deleted ? Ok() : NoContent();
    }

    #region Map

    private static Patient Map(AddPatientRequest request)
    {
        return new Patient
        {
            Name = new Patient.PatientName
            {
                Id = request.Name.Id,
                Use = request.Name.Use,
                Family = request.Name.Family,
                Given = request.Name.Given,
            },
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            IsActive = request.IsActive,
        };
    }

    private static Patient Map(UpdatePatientRequest request)
    {
        return new Patient
        {
            Name = new Patient.PatientName
            {
                Use = request.Name.Use,
                Family = request.Name.Family,
                Given = request.Name.Given,
            },
            Gender = request.Gender,
            BirthDate = request.BirthDate,
            IsActive = request.IsActive,
        };
    }

    #endregion Map
}
