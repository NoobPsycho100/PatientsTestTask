using PatientsTestTask.Core;

namespace PatientsTestTask.Tests;

[TestClass]
public sealed class DateFilterExtensionsTests
{
    [TestMethod]
    [DataRow("eq2013-01-14", true)]
    [DataRow("ne2013-01-14", true)]
    [DataRow("lt2013-01-14T10:00", true)]
    [DataRow("gt2013-01-14T10:00", true)]
    [DataRow("ge2013-03-14", true)]
    [DataRow("le2013-03-14", true)]
    [DataRow("sa2013-03-14", true)]
    [DataRow("eb2013-03-14", true)]
    [DataRow("ap2013-03-14", false)] // prefix ap currently not implemented
    [DataRow("2013-01-14", true)] // no prefix means eq
    [DataRow("eqq2013-01-14", false)]
    [DataRow("e2013-01-14", false)]
    [DataRow("eq", false)]
    public void IsCorrectDateFilterTest(string filter, bool expected)
    {
        var actual = filter.IsCorrectDateFilter();

        Assert.AreEqual(expected, actual);
    }

    // https://www.hl7.org/fhir/search.html#date
    [TestMethod]
    [DataRow("eq2013-01-14", "2013-01-14T00:00", true)]
    [DataRow("eq2013-01-14", "2013-01-14T10:00", true)]
    [DataRow("eq2013-01-14", "2013-01-15T00:00", false)]
    [DataRow("2013-01-14", "2013-01-14T00:00", true)] // no prefix means eq
    [DataRow("2013-01-14", "2013-01-14T10:00", true)]
    [DataRow("2013-01-14", "2013-01-15T00:00", false)]

    [DataRow("ne2013-01-14", "2013-01-15T00:00", true)]
    [DataRow("ne2013-01-14", "2013-01-14T00:00", false)]
    [DataRow("ne2013-01-14", "2013-01-14T10:00", false)]
    [DataRow("lt2013-01-14T10:00", "2013-01-14T00:00", true)]
    [DataRow("lt2013-01-14T10:00", "2013-01-13T12:00", true)]
    [DataRow("lt2013-01-14T10:00", "2013-01-14T12:00", false)]
    [DataRow("lt2013-01-14T10:00", "2013-01-14T08:00", true)]
    [DataRow("lt2013-01-14T10:00", "2013-01-15T08:00", false)]
    [DataRow("gt2013-01-14T10:00", "2013-01-15T00:00", true)]
    [DataRow("gt2013-01-14T10:00", "2013-01-14T00:00", false)]
    [DataRow("gt2013-01-14T10:00", "2013-01-13T12:00", false)]
    [DataRow("gt2013-01-14T10:00", "2013-01-14T12:00", true)]
    [DataRow("gt2013-01-14T10:00", "2013-01-15T12:00", true)]

    [DataRow("gt2013-01-14T00:00", "2013-01-14T00:00", false)]
    [DataRow("gt2013-01-14T00:00", "2013-01-14T10:00", true)]
    [DataRow("gt2013-01-14", "2013-01-14T10:00", false)]
    [DataRow("le2013-01-14T00:00", "2013-01-14T00:00", true)]
    [DataRow("le2013-01-14T00:00", "2013-01-14T10:00", false)]
    [DataRow("le2013-01-14", "2013-01-14T10:00", true)]
    public void IsDateMatchFilterTest(string filter, string dateString, bool expected)
    {
        var date = DateTime.Parse(dateString);
        var actual = filter.IsDateMatchFilter(date);

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    [DataRow("eq2013-01-14", "2013-01-14T00:00", true)]
    [DataRow("eq2013-01-14", "2013-01-14T10:00", true)]
    [DataRow("eq2013-01-14", "2013-01-15T00:00", false)]
    [DataRow("2013-01-14", "2013-01-14T00:00", true)] // no prefix means eq
    [DataRow("2013-01-14", "2013-01-14T10:00", true)]
    [DataRow("2013-01-14", "2013-01-15T00:00", false)]

    [DataRow("ne2013-01-14", "2013-01-15T00:00", true)]
    [DataRow("ne2013-01-14", "2013-01-14T00:00", false)]
    [DataRow("ne2013-01-14", "2013-01-14T10:00", false)]
    [DataRow("lt2013-01-14T10:00", "2013-01-14T00:00", true)]
    [DataRow("lt2013-01-14T10:00", "2013-01-13T12:00", true)]
    [DataRow("lt2013-01-14T10:00", "2013-01-14T12:00", false)]
    [DataRow("lt2013-01-14T10:00", "2013-01-14T08:00", true)]
    [DataRow("lt2013-01-14T10:00", "2013-01-15T08:00", false)]
    [DataRow("gt2013-01-14T10:00", "2013-01-15T00:00", true)]
    [DataRow("gt2013-01-14T10:00", "2013-01-14T00:00", false)]
    [DataRow("gt2013-01-14T10:00", "2013-01-13T12:00", false)]
    [DataRow("gt2013-01-14T10:00", "2013-01-14T12:00", true)]
    [DataRow("gt2013-01-14T10:00", "2013-01-15T12:00", true)]

    [DataRow("gt2013-01-14T00:00", "2013-01-14T00:00", false)]
    [DataRow("gt2013-01-14T00:00", "2013-01-14T10:00", true)]
    [DataRow("gt2013-01-14", "2013-01-14T10:00", false)]
    [DataRow("le2013-01-14T00:00", "2013-01-14T00:00", true)]
    [DataRow("le2013-01-14T00:00", "2013-01-14T10:00", false)]
    [DataRow("le2013-01-14", "2013-01-14T10:00", true)]
    public void ToDateFilterExpressionTest(string filter, string dateString, bool expected)
    {
        var date = DateTime.Parse(dateString);
        var objectWithDate = new ObjectWithDate { Date = date };

        var expression = filter.ToDateFilterExpression<ObjectWithDate>(x => x.Date);
        var actual = expression.Compile()(objectWithDate);

        Assert.AreEqual(expected, actual);
    }

    private class ObjectWithDate
    {
        public DateTime Date { get; set; } 
    }
}
