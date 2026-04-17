using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using PatientsTestTask.Core;

namespace PatientsTestTask.Web;

public class ErrorsFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ValidationException error)
        {
            HandleValidationError(context, error);
        }

        if (context.Exception is DbUpdateException dbError)
        {
            if (dbError.InnerException is SqlException sqlError)
            {
                if (sqlError.Message.Contains("FOREIGN KEY"))
                {
                    HandleValidationError(context, new ValidationException("Error while saving to database", field: "", error: "Invalid key"));
                }

                if (sqlError.Message.Contains("PRIMARY KEY"))
                {
                    HandleValidationError(context, new ValidationException("Error while saving to database", field: "", error: "Key already exists"));
                }
            }
        }
    }

    private void HandleValidationError(ExceptionContext context, ValidationException? error)
    {
        if (error != null)
        {
            context.Result = context.HttpContext.ValidationErrorResult(error);
            context.ExceptionHandled = true;
        }
    }
}

public static class ValidationExceptionExtensions
{
    public static BadRequestObjectResult ValidationErrorResult(this HttpContext context, ValidationException error)
    {
        return new BadRequestObjectResult(new
        {
            title = error.Message,
            status = 400,
            errors = error.FieldErrors.GroupBy(f => f.Field, f => f.Error).ToDictionary(x => x.Key, x => x.ToList()),
            traceId = context.TraceIdentifier,
        });
    }

    public static BadRequestObjectResult ValidationErrorResult(this HttpContext context, string field, string error)
    {
        return context.ValidationErrorResult(new ValidationException("One or more validation errors occurred.", field, error));
    }
}