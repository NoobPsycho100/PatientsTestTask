using FluentValidation;
using PatientsTestTask.Core.Services;
using PatientsTestTask.Web.Model;

namespace PatientsTestTask.Web.Validation;

public class AddPatientValidator: AbstractValidator<AddPatientRequest>
{
    public AddPatientValidator(IPatientsService patientsService)
    {
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("BirthDate field should not be empty");

        RuleFor(x => x.Name.Family).NotEmpty().WithMessage("Name.Family field should not be empty");

        RuleFor(x => x.Name.Id).NotEmpty().WithMessage("Name.Family field should not be empty");

        RuleFor(x => x.Name.Id).MustAsync(async (x, cancellation) => x == null || !await patientsService.IsPatientExists(x.Value))
                               .WithMessage("Specified id is alredy taken");
    }
}
