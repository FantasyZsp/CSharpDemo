using System;
using System.Text.Json;
using Common.Dto;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TestProject.Jsons;

public class JsonSeTests
{
    private readonly ITestOutputHelper _testOutputHelper;

    public JsonSeTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public void SeExtends()
    {
        var animal = new Animal() {Name = "Animal"};
        Animal dog = new Dog() {Name = "Animal", NickName = "Dog"};
        Animal cat = new Cat() {Name = "Animal", LovelyName = "Cat"};
        _testOutputHelper.WriteLine(JsonConvert.SerializeObject(animal));
        _testOutputHelper.WriteLine(JsonConvert.SerializeObject(dog));
        _testOutputHelper.WriteLine(JsonConvert.SerializeObject(cat));
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(animal));
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(dog));
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(cat,new JsonSerializerOptions()
        {
            IncludeFields = true,
            WriteIndented = true,
        }));
    }
}