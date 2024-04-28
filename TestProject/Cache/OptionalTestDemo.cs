using System.Threading.Tasks;
using TestProject.BaseApi.Models;
using Xunit;

namespace TestProject.Cache;
#nullable enable
public class OptionalTestDemo
{
    [Fact]
    public async Task OptionalTest()
    {
        // var optional1 = new Microsoft.CodeAnalysis.Optional<int?>();
        // Assert.False(optional1.HasValue);
        // Assert.Null(optional1.Value);

        await Task.CompletedTask;
        Optional<int> one = 1;
        Optional<int?> oneNullable = 1;
        Assert.True(one == oneNullable);


        Optional<int> two = 2;
        Optional<int?> twoNullable = null;
        Optional<int?> twoNullableCast = (int?) null;
        Assert.False(two == twoNullable);
        Assert.False(two == twoNullableCast);

        Optional<int?> twoNullable2 = null;
        Assert.True(twoNullable2.HasValue);
        Assert.True(twoNullable2.ValueIsNull);
        Assert.True(twoNullable2 == twoNullable);

        var optional = new Optional<int>();
        Assert.True(optional == 0);
        Assert.True(optional.HasValue);
        Assert.False(optional.ValueIsNull);

        var optional3 = new Optional<int?>();
        Optional<int?> optional2 = optional3;
        Assert.True(optional2 == null);
        Assert.True(optional2.Value == null);
        Assert.True(optional.HasValue);
        Assert.False(optional.ValueIsNull);

        Assert.False(new Optional<int?>() == new Optional<int>());
        Assert.True(Optional<int?>.FromValue(0) == new Optional<int>());
    }

    [Fact]
    public async Task OptionalTest2()
    {
        await Task.CompletedTask;
        var optionalKid = new Optional<Kid>();
        Assert.True(optionalKid == null!);
        Assert.True(optionalKid.Value == null);
        Assert.True(optionalKid.HasValue);
        Assert.True(optionalKid.ValueIsNull);


        Kid? optionalKid2 = Optional<Kid?>.FromValue(Kid.DefaultGGKid);
        Assert.True(optionalKid2 != null);

        Kid? optionalKid3 = Optional<Kid?>.FromValue(Kid.DefaultGGKid);
        Assert.True(optionalKid2 == optionalKid3);


        Kid? fromValue = Optional<Kid>.FromValue(optionalKid3);

        Assert.True(fromValue == optionalKid3);
        var fromValue2 = Optional<Kid>.FromValue(null);
        Assert.True(fromValue2 == null);
        Assert.True(fromValue2.Value == null);
        Assert.True(fromValue2.HasValue);
        Assert.True(fromValue2.ValueIsNull);
    }

    [Fact]
    public async Task OptionalTestEqualsWithHasValue()
    {
        await Task.CompletedTask;
        var optionalKid = Optional<Kid?>.FromValue(Kid.DefaultGGKid, false);
        Assert.True(optionalKid == Kid.DefaultGGKid);


        var number = Optional<int>.FromValue(1, false);
        Assert.True(number == 1);
    }


    [Fact]
    public async Task OptionalTestEqualsWithHasValue2()
    {
        await Task.CompletedTask;
        var optionalKid = Optional<Kid?>.FromValue(Kid.DefaultGGKid, true);
        Assert.True(optionalKid == Kid.DefaultGGKid);

        var number = Optional<int>.FromValue(1, true);
        Assert.True(number == 1);

        Optional<int?> nullable = null;
        Optional<int?> nullable2 = Optional<int?>.FromValue(null);
        Optional<int?> nullable3False = Optional<int?>.FromValue(null, false);

        Assert.True(nullable == nullable2);
        Assert.True(nullable == null);
        Assert.True(nullable3False == null);

        Optional<int?> nullable3True = Optional<int?>.FromValue(null, true);
        Assert.True(nullable3True == null);
    }
}