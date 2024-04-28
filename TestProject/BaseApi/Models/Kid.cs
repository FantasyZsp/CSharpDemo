using System;
using Newtonsoft.Json;

namespace TestProject.BaseApi.Models;

public class Kid
{
    public static readonly Kid DefaultGGKid = new Kid()
    {
        Name = "defaultKid",
        Age = 18,
        Sex = SexEnum.GG
    };

    public string Name { get; set; }
    public int Age { get; set; }
    public SexEnum Sex { get; set; }

    public static Kid Of(string name, int age = 18, SexEnum sex = SexEnum.GG)
    {
        return new Kid()
        {
            Name = name,
            Age = age,
            Sex = sex,
        };
    }


    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}

public enum SexEnum
{
    MM,
    GG
}

public class KidException : Exception
{
    public KidException(string message) : base(message)
    {
    }
}