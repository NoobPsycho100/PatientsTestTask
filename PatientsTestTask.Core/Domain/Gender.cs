using System.Text.Json.Serialization;

namespace PatientsTestTask.Core.Domain;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Gender
{
    male,
    female,
    other,
    unknown
}