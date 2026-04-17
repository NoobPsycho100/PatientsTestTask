using System.Linq.Expressions;

namespace PatientsTestTask.Core;

/// <summary>
/// Extensions for date filter: https://www.hl7.org/fhir/search.html#date
/// </summary>
public static class DateFilterExtensions
{
    public static bool IsCorrectDateFilter(this string filter)
    {
        return TryParse(filter, out _, out _, out _);
    }

    public static bool IsDateMatchFilter(this string filter, DateTime date)
    {
        if (!TryParse(filter, out string operation, out DateTime filterDate, out bool isDayOnly))
            throw new ArgumentException("Incorrect date filter format");

        var filterNextDay = filterDate.AddDays(1);
        return operation switch
        {
            "eq" => isDayOnly ? date >= filterDate && date < filterNextDay : date == filterDate,
            "ne" => isDayOnly ? date < filterDate || date >= filterNextDay : date != filterDate,
            "gt" => isDayOnly ? date >= filterNextDay : date > filterDate,
            "lt" => date < filterDate,
            "ge" => date >= filterDate,
            "le" => isDayOnly ? date < filterNextDay : date <= filterDate,
            "sa" => isDayOnly ? date >= filterNextDay : date >= filterDate,
            "eb" => date < filterDate,
            _ => throw new NotImplementedException("Unknown date filter format prefix")
        };
    }

    public static Expression<Func<T, bool>> ToDateFilterExpression<T>(this string filter, Expression<Func<T, DateTime>> property)
    {
        if (!TryParse(filter, out string operation, out DateTime filterDate, out bool isDayOnly))
            throw new ArgumentException("Incorrect date filter format");

        var filterNextDay = filterDate.AddDays(1);
        Expression <Func<T, DateTime>> dateExpression = x => filterDate;
        Expression<Func<T, DateTime>> nextDateExpression = x => filterNextDay;

        var result = operation switch
        {
            "eq" => isDayOnly ?
                    Expression.And(Expression.GreaterThanOrEqual(property.Body, dateExpression.Body), Expression.LessThan(property.Body, nextDateExpression.Body)) :
                    Expression.Equal(property.Body, dateExpression.Body),
            "ne" => isDayOnly ?
                    Expression.Or(Expression.LessThan(property.Body, dateExpression.Body), Expression.GreaterThanOrEqual(property.Body, nextDateExpression.Body)) :
                    Expression.NotEqual(property.Body, dateExpression.Body),
            "gt" => isDayOnly ?
                    Expression.GreaterThanOrEqual(property.Body, nextDateExpression.Body) :
                    Expression.GreaterThan(property.Body, dateExpression.Body),
            "lt" => Expression.LessThan(property.Body, dateExpression.Body),
            "ge" => Expression.GreaterThanOrEqual(property.Body, dateExpression.Body),
            "le" => isDayOnly ?
                    Expression.LessThan(property.Body, nextDateExpression.Body) :
                    Expression.LessThanOrEqual(property.Body, dateExpression.Body),
            "sa" => isDayOnly ?
                        Expression.GreaterThanOrEqual(property.Body, nextDateExpression.Body) :
                        Expression.GreaterThanOrEqual(property.Body, dateExpression.Body),
            "eb" => Expression.LessThan(property.Body, dateExpression.Body),
            _ => throw new NotImplementedException("Unknown date filter format prefix")
        };
        
        var resultLambda = Expression.Lambda<Func<T, bool>>(result, property.Parameters);
        return resultLambda;
    }

    private static bool TryParse(string filter, out string operation, out DateTime date, out bool isDayOnly)
    {
        filter = filter.Trim();

        if (string.IsNullOrEmpty(filter) || filter.Length < 2)
        {
            operation = "eq";
            date = default;
            isDayOnly = true;
            return false;
        }

        var prefix = filter.Substring(0, 2);
        var datePart = filter.Substring(2);

        (operation, datePart) = prefix switch
        {
            "eq" => ("eq", datePart),
            "ne" => ("ne", datePart),
            "gt" => ("gt", datePart),
            "lt" => ("lt", datePart),
            "ge" => ("ge", datePart),
            "le" => ("le", datePart),
            "sa" => ("sa", datePart),
            "eb" => ("eb", datePart),
            _ => ("eq", filter) // no prefix means eq operation
        };

        isDayOnly = !datePart.Contains('T');
        return DateTime.TryParse(datePart, out date);
    }
}
