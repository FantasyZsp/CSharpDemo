using System;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Annotations;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class BaseAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class InheritedAttribute : BaseAttribute
{
}

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DisableInheritedAttribute : BaseAttribute
{
}

[Base]
public class BaseClass
{
}

public class BaseSubClass : BaseClass
{
}

[Inherited]
public class InheritedClass
{
}

public class InheritedSubClass : InheritedClass
{
}

[DisableInherited]
public class DisableInheritedClass
{
}

public class DisableInheritedSubClass : DisableInheritedClass
{
}

public class AttributeInheritedDemo
{
    private readonly ITestOutputHelper _testOutputHelper;

    public AttributeInheritedDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void TestDemo()
    {
        var baseClassType = typeof(BaseClass);
        var inheritMethodTrue = baseClassType.IsDefined(typeof(BaseAttribute), true);
        var inheritMethodFalse = baseClassType.IsDefined(typeof(BaseAttribute), false);
        _testOutputHelper.WriteLine($"BaseClass {inheritMethodTrue} {inheritMethodFalse}");


        var baseSubClassType = typeof(BaseSubClass);
        var baseSubClassTypeFlag = baseSubClassType.IsDefined(typeof(BaseAttribute), true);
        var baseSubClassTypeFlag2 = baseSubClassType.IsDefined(typeof(BaseAttribute), false);
        _testOutputHelper.WriteLine($"BaseSubClass {baseSubClassTypeFlag} {baseSubClassTypeFlag2}");


        var inheritedClassType = typeof(InheritedClass);
        var inheritedClassTypeFlag1 = inheritedClassType.IsDefined(typeof(BaseAttribute), true);
        var inheritedClassTypeFlag2 = inheritedClassType.IsDefined(typeof(BaseAttribute), false);
        _testOutputHelper.WriteLine($"InheritedClass {inheritedClassTypeFlag1} {inheritedClassTypeFlag2}");


        var inheritedSubClassType = typeof(InheritedSubClass);
        var inheritedSubClassTypeFlag1 = inheritedSubClassType.IsDefined(typeof(BaseAttribute), true);
        var inheritedSubClassTypeFlag2 = inheritedSubClassType.IsDefined(typeof(BaseAttribute), false);
        _testOutputHelper.WriteLine($"InheritedSubClass {inheritedSubClassTypeFlag1} {inheritedSubClassTypeFlag2}");

        var disableClassType = typeof(DisableInheritedClass);
        var inheritMethodTrue3 = disableClassType.IsDefined(typeof(BaseAttribute), true);
        var inheritMethodFalse3 = disableClassType.IsDefined(typeof(BaseAttribute), false);
        _testOutputHelper.WriteLine($"DisableInheritedClass {inheritMethodTrue3} {inheritMethodFalse3}");

        var finalCaseType = typeof(DisableInheritedSubClass);
        var inheritMethodTrue4 = finalCaseType.IsDefined(typeof(BaseAttribute), true);
        var inheritMethodFalse4 = finalCaseType.IsDefined(typeof(BaseAttribute), false);
        _testOutputHelper.WriteLine($"DisableInheritedSubClass {inheritMethodTrue4} {inheritMethodFalse4}");
    }
}