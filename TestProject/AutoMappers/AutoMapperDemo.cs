using System;
using AutoMapper;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.AutoMappers;

public class AutoMapperDemo
{
    private static readonly IMapper Mapper = new MapperConfiguration(cfg => { cfg.CreateMap<object, object>(); }).CreateMapper();
    
    private readonly ITestOutputHelper _testOutputHelper;

    public AutoMapperDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }


    [Fact]
    public void Map()
    {
        var mapper = new MapperConfiguration(cfg => { cfg.CreateMap<PersonA, PersonB>(); }).CreateMapper();
        var personB = mapper.Map<PersonB>(new PersonA { Name = "xxx" });
        
        _testOutputHelper.WriteLine(JsonConvert.SerializeObject(personB));
    }
}

public class PersonA
{
    public string Name { get; set; }
}

public class PersonB
{
    public string Name { get; set; }
}