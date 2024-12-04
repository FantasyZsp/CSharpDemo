using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TestProject.BaseApi.Models;
#nullable enable
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
    [JsonConverter(typeof(StringEnumConverter))]
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

    public bool EqualsValue(Kid other)
    {
        return Name == other.Name && Age == other.Age && Sex == other.Sex;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return EqualsValue((Kid) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Age, (int) Sex);
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