namespace PatientsTestTask.Core;

public class ValidationException: Exception
{
    public record FieldError(string Field, string Error);

    public FieldError[] FieldErrors { get; private init; }

    public ValidationException(string message, params FieldError[] fieldErrors): base(message)
    {
        FieldErrors = fieldErrors ?? [];
    }

    public ValidationException(string message, string field, string error) : base(message)
    {
        FieldErrors = [new FieldError(field, error)];
    }
}
