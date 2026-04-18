using PatientsTestTask.Core.Domain;
using System.Net.Http.Json;
using System.Text.Json;

var baseUrl = "http://localhost:5000";
var addPatientUrl = $"{baseUrl}/patients";
var client = new HttpClient();

string[] maleFamilies = ["Иванов", "Петров", "Смит", "Козлов", "Новиков", "Карпович", "Мурашко", "Бойко", "Громыко"];
string[] femaleFamilies = ["Иванова", "Петрова", "Смит", "Козлова", "Новикова", "Карпович", "Мурашко", "Бойко", "Громыко"];
string[] maleNames = ["Иван", "Петр", "Джон", "Антон", "Артемий", "Яков", "Семён", "Кирилл", "Святослав"];
string[] femaleNames = ["Анна", "Галина", "Дженна", "Ялина", "Вероника", "Алина", "Екатерина", "Тамара", "Полина"];
string[] maleSecondNames = ["Иванович", "Петрович", "Джонович", "Антонович", "Артемьевич", "Яковлевич", "Семёнович", "Кириллович", "Святославович"];
string[] femaleSecondNames = ["Ивановна", "Петровна", "Джоновна", "Антоновна", "Артемьевна", "Яковлевна", "Семёновна", "Кирилловна", "Святославовна"];

Console.OutputEncoding = System.Text.Encoding.Unicode;
JsonSerializerOptions jso = new JsonSerializerOptions();
jso.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
Console.WriteLine("Adding 100 patients to api:");

for (int i = 0; i < 100;  i++)
{
    var patient = GeneratePatient();

    var response = await client.PostAsJsonAsync(addPatientUrl, patient);
    response.EnsureSuccessStatusCode();

    Console.WriteLine($"Added {i} patients:");
    Console.WriteLine(JsonSerializer.Serialize(patient, jso));
}

Patient GeneratePatient()
{
    var gender = Random.Shared.Next(1) == 1 ? Gender.male : Gender.female;
    var family = gender == Gender.male ? maleFamilies[Random.Shared.Next(maleFamilies.Length - 1)] : femaleFamilies[Random.Shared.Next(femaleFamilies.Length - 1)];
    var name = gender == Gender.male ? maleNames[Random.Shared.Next(maleNames.Length - 1)] : femaleNames[Random.Shared.Next(femaleNames.Length - 1)];
    var secondName = gender == Gender.male ? maleSecondNames[Random.Shared.Next(maleSecondNames.Length - 1)] : femaleSecondNames[Random.Shared.Next(femaleSecondNames.Length - 1)];
    return new Patient
    {
        // BirthDate from DateTime.Now-10 day to DateTime.Now
        BirthDate = DateTime.Now.AddSeconds(-Random.Shared.Next(864000)),
        Gender = gender,
        IsActive = true,
        Name = new Patient.PatientName
        {
            Family = family,
            Use = "official",
            Given = [name, secondName],
        }
    };
}
