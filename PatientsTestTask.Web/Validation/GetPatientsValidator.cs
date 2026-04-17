using FluentValidation;
using PatientsTestTask.Core;
using PatientsTestTask.Core.Services;
using PatientsTestTask.Web.Model;

namespace PatientsTestTask.Web.Validation;

public class GetPatientsValidator: AbstractValidator<GetPatientsRequest>
{
    public GetPatientsValidator(IPatientsService patientsService)
    {
        RuleForEach(x => x.BirthDate).NotEmpty().WithMessage("BirthDate field should not be empty");

        RuleForEach(x => x.BirthDate).Must(x => x.IsCorrectDateFilter()).WithMessage("BirthDate filter is in incorrect format");
    }
}
