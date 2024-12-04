using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TestProject.FreeSql;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.BaseApi
{
    public class SerializeTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SerializeTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public static T DeepCopy<T>(T obj)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(obj));
        }

        [Fact]
        public void Test()
        {
            var customer = new Customer
            {
                Id = null,
                Name = "222-33",
                Email = "testEmail4",
                CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var deepCopy = DeepCopy(customer);
            _testOutputHelper.WriteLine((deepCopy == customer).ToString());
        }

        [Fact]
        public void Test33()
        {
            var customer = new CustomerDto()
            {
                ID = 1,
                Name = "222-33",
            };

            var serializeObject = JsonConvert.SerializeObject(customer, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            _testOutputHelper.WriteLine(serializeObject);
            serializeObject = JsonConvert.SerializeObject(customer);
            _testOutputHelper.WriteLine(serializeObject);
        }

        [Fact]
        public void TestS()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            string contents = "{\"Id\":null,\"Name\":\"222-33\",\"Email\":\"testEmail4\",\"CardId\":1004,\"Mark\":\"mark4\",\"Config\":\"config4\"}\n";

            var serializeObject = JsonConvert.SerializeObject(contents);

            _testOutputHelper.WriteLine(serializeObject);

            var jObject = new JObject
            {
                ["Id"] = 1
            };

            var fromObject = JObject.FromObject(jObject, new JsonSerializer()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            _testOutputHelper.WriteLine(fromObject.ToString());
        }

        [Fact]
        public void TestS2()
        {
            string json = "{\"FirstName\":\"John\",\"LastName\":\"Doe\"}";
            var contractResolver = new CamelCasePropertyNamesContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy
                {
                    ProcessDictionaryKeys = true,
                    OverrideSpecifiedNames = true
                }
            };
            var settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver
            };
            // 反序列化JSON字符串
            var obj = JsonConvert.DeserializeObject<JObject>(json, settings);
            string newJson = JsonConvert.SerializeObject(obj, settings);
            _testOutputHelper.WriteLine(newJson); // 依然大驼峰
        }

        [Fact]
        void TestS3()
        {
            string jsonString =
                "{\"FloorTileThroughout\": {\n    \"FloorTilesThroughoutPlans\": [\n      {\n        \"TileMode\": {\n          \"TileMetaPath\": \"铺贴库/4ac184b986b24746973e9bab8f99769c\",\n          \"PolygonMaterialMapper\": {},\n          \"Materials\": [\n            {\n              \"ID\": \"a88831eb4a2b48db80b23ae3d690959c\",\n              \"Path\": \"材质库/a88831eb4a2b48db80b23ae3d690959c\",\n              \"SubPath\": \"/Materials/罗马米白\",\n              \"OriginalSubPath\": null,\n              \"MaterialType\": 0,\n              \"IsUserMaterial\": false,\n              \"Width\": 3000,\n              \"Length\": 3000,\n              \"MeshSectionId\": null,\n              \"Color\": null,\n              \"MaterialTextureOrientation\": {\n                \"X\": 0,\n                \"Y\": 1\n              },\n              \"CanCut\": false,\n              \"TextureRotation\": 0,\n              \"TextureScaleU\": 1,\n              \"TextureScaleV\": 1\n            },\n            {\n              \"ID\": \"6c6b06dd01dea06c1e9c602c6d22088c\",\n              \"Path\": \"材质库/6c6b06dd01dea06c1e9c602c6d22088c\",\n              \"SubPath\": \"/Materials/MI_杏黄色美缝剂\",\n              \"OriginalSubPath\": null,\n              \"MaterialType\": 1,\n              \"IsUserMaterial\": false,\n              \"Width\": 3000.0,\n              \"Length\": 3000.0,\n              \"MeshSectionId\": null,\n              \"Color\": null,\n              \"MaterialTextureOrientation\": {\n                \"X\": 0.0,\n                \"Y\": 0.0\n              },\n              \"CanCut\": false,\n              \"TextureRotation\": 0.0,\n              \"TextureScaleU\": 1.0,\n              \"TextureScaleV\": 1.0\n            }\n          ],\n          \"TileType\": 0,\n          \"StartPoint3D\": null,\n          \"UnitStyleReferenceType\": null,\n          \"StartPointType\": 7,\n          \"GapWidth\": 0.3,\n          \"Parameters\": {\n            \"L0\": 80,\n            \"L1\": 80\n          },\n          \"OffsetX\": 0,\n          \"OffsetY\": 0,\n          \"Rotation\": 0,\n          \"OriginalRotation\": null,\n          \"TileGuideDirection\": null\n        },\n        \"RoomList\": [\n          \"LivingRoom\",\n          \"DiningRoom\",\n          \"Entrance\",\n          \"Passage\",\n          \"WashRoom\"\n        ]\n      },\n      {\n        \"TileMode\": {\n          \"TileMetaPath\": \"铺贴库/fa28f84fce494e07b0a5f27ca1b41e4a\",\n          \"PolygonMaterialMapper\": {},\n          \"Materials\": [\n            {\n              \"ID\": \"8a3e53d22aa811f9d7673e371f1b3382\",\n              \"Path\": \"材质库/8a3e53d22aa811f9d7673e371f1b3382\",\n              \"SubPath\": \"/Materials/MI_muwen\",\n              \"OriginalSubPath\": null,\n              \"MaterialType\": 0,\n              \"IsUserMaterial\": false,\n              \"Width\": 3000,\n              \"Length\": 3000,\n              \"MeshSectionId\": null,\n              \"Color\": null,\n              \"MaterialTextureOrientation\": {\n                \"X\": 0,\n                \"Y\": 1\n              },\n              \"CanCut\": false,\n              \"TextureRotation\": 0,\n              \"TextureScaleU\": 1,\n              \"TextureScaleV\": 1\n            },\n            {\n              \"ID\": \"89cf5c0fb02a29670521aa8ccbea2398\",\n              \"Path\": \"材质库/89cf5c0fb02a29670521aa8ccbea2398\",\n              \"SubPath\": \"/Materials/MI_银河美缝剂\",\n              \"OriginalSubPath\": null,\n              \"MaterialType\": 1,\n              \"IsUserMaterial\": false,\n              \"Width\": 3000.0,\n              \"Length\": 3000.0,\n              \"MeshSectionId\": null,\n              \"Color\": null,\n              \"MaterialTextureOrientation\": {\n                \"X\": 0.0,\n                \"Y\": 0.0\n              },\n              \"CanCut\": false,\n              \"TextureRotation\": 0.0,\n              \"TextureScaleU\": 1.0,\n              \"TextureScaleV\": 1.0\n            }\n          ],\n          \"TileType\": 0,\n          \"StartPoint3D\": null,\n          \"UnitStyleReferenceType\": null,\n          \"StartPointType\": 7,\n          \"GapWidth\": 0.2,\n          \"Parameters\": {\n            \"L0\": 160,\n            \"L1\": 20\n          },\n          \"OffsetX\": 0,\n          \"OffsetY\": 0,\n          \"Rotation\": 0,\n          \"OriginalRotation\": null,\n          \"TileGuideDirection\": null\n        },\n        \"RoomList\": [\n          \"MainBedroom\",\n          \"SecondaryBedroom\",\n          \"StudyRoom\",\n          \"TeaRoom\",\n          \"LeisureBalcony\",\n          \"Cloakroom\",\n          \"StorageRoom\",\n          \"ChildrenBedroom\",\n          \"BoyRoom\",\n          \"GirlRoom\",\n          \"ElderlyRoom\"\n        ]\n      }\n    ]\n  }}";

            JObject jObject = JObject.Parse(jsonString);
            JObject camelCaseJObject = ConvertToCamelCase(jObject);

            string outputJson = JsonConvert.SerializeObject(camelCaseJObject, Formatting.Indented);
            _testOutputHelper.WriteLine(outputJson);
        }

        static JObject ConvertToCamelCase(JObject jObject)
        {
            var newObject = new JObject();
            foreach (var property in jObject.Properties())
            {
                string camelCaseName = Char.ToLowerInvariant(property.Name[0]) + property.Name.Substring(1);

                if (property.Value is JObject nestedObject)
                {
                    newObject[camelCaseName] = ConvertToCamelCase(nestedObject);
                }
                else if (property.Value is JArray array)
                {
                    newObject[camelCaseName] = ConvertArray(array);
                }
                else
                {
                    newObject[camelCaseName] = property.Value;
                }
            }

            return newObject;
        }

        static JArray ConvertArray(JArray array)
        {
            var newArray = new JArray();
            foreach (var item in array)
            {
                if (item is JObject nestedObject)
                {
                    newArray.Add(ConvertToCamelCase(nestedObject));
                }
                else if (item is JArray nestedArray)
                {
                    newArray.Add(ConvertArray(nestedArray));
                }
                else
                {
                    newArray.Add(item);
                }
            }

            return newArray;
        }
    }
}