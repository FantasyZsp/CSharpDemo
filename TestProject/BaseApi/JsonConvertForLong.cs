using System;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi;

public class JsonConvertForLong
{
    private readonly ITestOutputHelper _testOutputHelper;

    public JsonConvertForLong(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public class ValueToStringConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ulong) || objectType == typeof(ulong?) || objectType == typeof(long) || objectType == typeof(long?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => throw new NotSupportedException();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var str = value?.ToString();
            writer.WriteValue(str);
        }
    }


    public class Model
    {
        [JsonConverter(typeof(ValueToStringConverter))]
        public ulong ULongOne { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public ulong? ULongNullableOne { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long LongOne { get; set; }

        [JsonConverter(typeof(ValueToStringConverter))]
        public long? LongNullableOne { get; set; }

        public int? IntOne { get; set; }
        public uint? UIntOne { get; set; }
    }

    [Fact]
    public void Test()
    {
        var m = new Model
        {
            ULongOne = 1111111111111111111L,
            ULongNullableOne = 932384626433L,
            LongOne = 932384626433L,
            LongNullableOne = 932384626433L,
            IntOne = 1234567890,
            UIntOne = 1234567890
        };

        var json = JsonConvert.SerializeObject(m);
        _testOutputHelper.WriteLine(json);

        _testOutputHelper.WriteLine("----------------------------------------");

        var deserializationResult = JsonConvert.DeserializeObject<Model>(json);

        _testOutputHelper.WriteLine($"{m.ULongOne == deserializationResult.ULongOne}");
        _testOutputHelper.WriteLine($"{m.ULongNullableOne == deserializationResult.ULongNullableOne}");
        _testOutputHelper.WriteLine($"{m.LongOne == deserializationResult.LongOne}");
        _testOutputHelper.WriteLine($"{m.LongNullableOne == deserializationResult.LongNullableOne}");
        _testOutputHelper.WriteLine($"{m.IntOne == deserializationResult.IntOne}");
        _testOutputHelper.WriteLine($"{m.UIntOne == deserializationResult.UIntOne}");
        Thread.Sleep(10000);
    }


    [Fact]
    public void TestString()
    {
        const string test = "  test      xxxx wwww   dddd   \n testenter  \t testtt ";
        _testOutputHelper.WriteLine(test.Replace(" ", "").Replace("    ", ""));
        _testOutputHelper.WriteLine(Regex.Replace(test, @"\s+", ""));
    }
}