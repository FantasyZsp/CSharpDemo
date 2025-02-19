using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.Threads;

public class ForkJoinDemo
{
    private readonly ITestOutputHelper _testOutputHelper;

    public ForkJoinDemo(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task ForkJoinSumTest()
    {
        var forkJoinSum = new ForkJoinSum(Enumerable.Range(1, 100000).Select(i => long.Parse(i.ToString())).ToArray());
        _testOutputHelper.WriteLine((await forkJoinSum.Compute()).ToString());
    }
}

class ForkJoinSum
{
    private readonly long[] _numbers;

    public ForkJoinSum(long[] numbers)
    {
        this._numbers = numbers;
    }

    // 对外公开的方法，开始计算总和
    public async Task<long> Compute()
    {
        return await ForkJoin(0, _numbers.Length - 1);
    }

    // 递归进行任务分割（类似Fork）并合并结果（类似Join）
    private async Task<long> ForkJoin(long start, long end)
    {
        // 如果范围足够小，直接计算该范围的和（相当于不再分割任务）
        if (end - start <= 100)
        {
            var sum = 0L;
            for (var i = start; i <= end; i++)
            {
                sum += _numbers[i];
            }

            return sum;
        }

        // 分割任务
        var mid = start + (end - start) / 2;

        // 创建两个子任务，分别计算左右两部分的和
        var leftTask = Task.Run(() => ForkJoin(start, mid));
        var rightTask = Task.Run(() => ForkJoin(mid + 1, end));

        // 等待子任务完成并合并结果（类似Join操作）
        return await leftTask + await rightTask;
    }
}

public class Program
{
    public static async Task Test()
    {
        string jsonFilePath = "your_file_path.json"; // 替换为实际的JSON文件路径
        var jsonContent = File.ReadAllText(jsonFilePath);
        var root = JsonConvert.DeserializeObject<RootObject>(jsonContent);

        var checker = new JsonReferenceChecker();
        var results = await checker.CheckAllReferences(root);

        foreach (var result in results)
        {
            if (result is Product product)
            {
                Console.WriteLine($"产品: {product.Id}，被 {string.Join(", ", product.ReferencedBy)} 引用，引用检查{(product.IsReferenceValid ? "通过" : "失败")}");
            }
            else if (result is Structure structure)
            {
                Console.WriteLine($"结构: {structure.Id}，被 {string.Join(", ", structure.ReferencedBy)} 引用，引用检查{(structure.IsReferenceValid ? "通过" : "失败")}");
            }
            else if (result is ProductGroup productGroup)
            {
                Console.WriteLine($"产品组: {productGroup.Id}，被 {string.Join(", ", productGroup.ReferencedBy)} 引用，引用检查{(productGroup.IsReferenceValid ? "通过" : "失败")}");
            }
        }
    }
}

class JsonReferenceChecker
{
    private async Task CheckReferencesForList<T>(List<T> items, string parentId) where T : class
    {
        var tasks = new List<Task>();
        foreach (var item in items)
        {
            tasks.Add(Task.Run(() =>
            {
                string filePath = GetFullPath(item);
                if (File.Exists(filePath))
                {
                    var jsonContent = File.ReadAllText(filePath);
                    var subObject = JsonConvert.DeserializeObject<RootObject>(jsonContent);

                    // 记录被哪个上层元素引用
                    if (item is Product product)
                    {
                        product.ReferencedBy.Add(parentId);
                    }
                    else if (item is Structure structure)
                    {
                        structure.ReferencedBy.Add(parentId);
                    }
                    else if (item is ProductGroup productGroup)
                    {
                        productGroup.ReferencedBy.Add(parentId);
                    }

                    // 并行检查子对象的引用（递归调用）
                    var subTasks = new List<Task>();
                    subTasks.Add(CheckReferencesForList(subObject.Products, item is Product p ? p.Id : ""));
                    subTasks.Add(CheckReferencesForList(subObject.Structures, item is Structure s ? s.Id : ""));
                    subTasks.Add(CheckReferencesForList(subObject.ProductGroups, item is ProductGroup pg ? pg.Id : ""));
                    Task.WaitAll(subTasks.ToArray());

                    // 根据子对象引用检查结果更新当前对象的检查结果
                    if (item is Product p1)
                    {
                        p1.IsReferenceValid = subTasks.All(t => t.IsCompletedSuccessfully);
                    }
                    else if (item is Structure s1)
                    {
                        s1.IsReferenceValid = subTasks.All(t => t.IsCompletedSuccessfully);
                    }
                    else if (item is ProductGroup pg1)
                    {
                        pg1.IsReferenceValid = subTasks.All(t => t.IsCompletedSuccessfully);
                    }
                }
                else
                {
                    // 文件不存在，设置当前对象引用检查结果为失败
                    if (item is Product p)
                    {
                        p.IsReferenceValid = false;
                    }
                    else if (item is Structure s)
                    {
                        s.IsReferenceValid = false;
                    }
                    else if (item is ProductGroup pg)
                    {
                        pg.IsReferenceValid = false;
                    }
                }
            }));
        }

        await Task.WhenAll(tasks);
    }

    private string GetFullPath(object item)
    {
        if (item is Product product)
        {
            return Path.Combine(product.Path, product.SubPath);
        }
        else if (item is Structure structure)
        {
            return structure.Path;
        }
        else if (item is ProductGroup productGroup)
        {
            return productGroup.Path;
        }

        return "";
    }

    public async Task<List<object>> CheckAllReferences(RootObject root)
    {
        var allTasks = new List<Task>();
        allTasks.Add(CheckReferencesForList(root.Products, root.ID));
        allTasks.Add(CheckReferencesForList(root.Structures, root.ID));
        allTasks.Add(CheckReferencesForList(root.ProductGroups, root.ID));

        await Task.WhenAll(allTasks);

        var allItems = root.Products.Cast<object>().ToList();
        allItems.AddRange(root.Structures.Cast<object>());
        allItems.AddRange(root.ProductGroups.Cast<object>());

        return allItems;
    }
}

// 整体的根结构类
class RootObject
{
    [JsonProperty("ID")] public string ID { get; set; }

    [JsonProperty("Products")] public List<Product> Products { get; set; }

    [JsonProperty("Structures")] public List<Structure> Structures { get; set; }

    [JsonProperty("ProductGroups")] public List<ProductGroup> ProductGroups { get; set; }
}

// 产品类，添加引用相关属性和检查结果属性
class Product
{
    [JsonProperty("Id")] public string Id { get; set; }
    [JsonProperty("SpecId")] public string SpecId { get; set; }
    [JsonProperty("Path")] public string Path { get; set; }
    [JsonProperty("SubPath")] public string SubPath { get; set; }
    [JsonProperty("CurtainType")] public string CurtainType { get; set; }

    // 记录被哪些上层元素引用（这里简单用字符串表示，可根据实际细化类型等信息）
    public List<string> ReferencedBy { get; set; } = new List<string>();

    // 检查结果标识，true表示引用检查通过，false表示失败
    public bool IsReferenceValid { get; set; }
}

// 结构类，添加引用相关属性和检查结果属性
class Structure
{
    [JsonProperty("Id")] public string Id { get; set; }
    [JsonProperty("Path")] public string Path { get; set; }

    public List<string> ReferencedBy { get; set; } = new List<string>();
    public bool IsReferenceValid { get; set; }
}

// 产品组类，添加引用相关属性和检查结果属性
class ProductGroup
{
    [JsonProperty("Id")] public string Id { get; set; }
    [JsonProperty("Path")] public string Path { get; set; }

    public List<string> ReferencedBy { get; set; } = new List<string>();
    public bool IsReferenceValid { get; set; }
}