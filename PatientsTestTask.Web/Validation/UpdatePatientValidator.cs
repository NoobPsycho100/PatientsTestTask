using FluentValidation;
using PatientsTestTask.Web.Model;

namespace PatientsTestTask.Web.Validation;

public class UpdatePatientValidator : AbstractValidator<UpdatePatientRequest>
{
    public UpdatePatientValidator()
    {
        RuleFor(x => x.BirthDate).NotEmpty().WithMessage("BirthDate field should not be empty");

        RuleFor(x => x.Name.Family).NotEmpty().WithMessage("Name.Family field should not be empty");
    }
}
