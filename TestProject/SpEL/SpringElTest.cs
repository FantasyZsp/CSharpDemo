using System;
using System.Collections;
using Spring.Expressions;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.SpEL;

public class SpringElTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public SpringElTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void Test()
    {
        Inventor tesla = new Inventor("Nikola Tesla", new DateTime(1856, 7, 9), "Serbian");

        tesla.PlaceOfBirth.City = "Smiljan";

        var evaluatedName = (string) ExpressionEvaluator.GetValue(tesla, "Name");

        var evaluatedCity = (string) ExpressionEvaluator.GetValue(tesla, "PlaceOfBirth.City");
        var testStr = (string) ExpressionEvaluator.GetValue(null, "'PlaceOfBirth.City'"); // 字面量用单引号即可，root给不给值都可以
        _testOutputHelper.WriteLine("value: " + evaluatedName);
        _testOutputHelper.WriteLine("value: " + evaluatedCity);
        _testOutputHelper.WriteLine("testStr: " + testStr);
    }
}

public class Inventor
{
    public string Name;
    public string Nationality;
    public string[] Inventions;
    private DateTime dob;
    private Place pob;

    public Inventor() : this(null, DateTime.MinValue, null)
    {
    }

    public Inventor(string name, DateTime dateOfBirth, string nationality)
    {
        this.Name = name;
        this.dob = dateOfBirth;
        this.Nationality = nationality;
        this.pob = new Place();
    }

    public DateTime DOB
    {
        get { return dob; }
        set { dob = value; }
    }

    public Place PlaceOfBirth
    {
        get { return pob; }
    }

    public int GetAge(DateTime on)
    {
        // not very accurate, but it will do the job ;-)
        return on.Year - dob.Year;
    }
}

public class Place
{
    public string City;
    public string Country;
}

public class Society
{
    public string Name;
    public static string Advisors = "advisors";
    public static string President = "president";

    private IList members = new ArrayList();
    private IDictionary officers = new Hashtable();

    public IList Members
    {
        get { return members; }
    }

    public IDictionary Officers
    {
        get { return officers; }
    }

    public bool IsMember(string name)
    {
        bool found = false;
        foreach (Inventor inventor in members)
        {
            if (inventor.Name == name)
            {
                found = true;
                break;
            }
        }

        return found;
    }
}