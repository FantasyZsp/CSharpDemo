using System;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace TestProject.MapsLngLat;

public class Demo
{
    public static string ConvertToPolygonString(decimal[] coordinates)
    {
        if (coordinates.Length % 2 != 0)
        {
            throw new ArgumentException("经纬度数组的长度必须为偶数。");
        }

        var polygonBuilder = new StringBuilder("(");

        for (var i = 0; i < coordinates.Length; i += 2)
        {
            var longitude = coordinates[i];
            var latitude = coordinates[i + 1];

            // 格式化经纬度并添加到多边形字符串中
            polygonBuilder.Append($"{longitude} {latitude}");

            // 如果不是最后一个点，添加逗号和空格
            if (i < coordinates.Length - 2)
            {
                polygonBuilder.Append(", ");
            }
        }

        // 添加封闭括号
        polygonBuilder.Append(')');
        return polygonBuilder.ToString();
    }

    public static async Task<JObject> GetPolygonByCode(string adCode)
    {
        var key = "A2IBZ-QTZ3J-SWFFK-XOAPZ-D64TE-IDBMB";
        
        var requestUrl = $"https://apis.map.qq.com/ws/district/v1/search?keyword=410100&get_polygon=1&key={key}&id={adCode}";
        var jsonString = await requestUrl.GetStringAsync();
        return JsonConvert.DeserializeObject<JObject>(jsonString, QqMapSerializerSettings);
    }

    public static readonly JsonSerializerSettings QqMapSerializerSettings = new()
    {
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        },
        //Formatting = Formatting.Indented
    };
}