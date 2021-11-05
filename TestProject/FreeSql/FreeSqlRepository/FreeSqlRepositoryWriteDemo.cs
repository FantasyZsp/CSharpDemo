using FreeSql;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.FreeSql.FreeSqlRepository
{
    public class FreeSqlRepositoryWriteDemo
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public FreeSqlRepositoryWriteDemo(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private IFreeSql CreateLocalFreeSql()
        {
            var fsql = new FreeSqlBuilder()
                .UseConnectionString(DataType.MySql, @"Server=localhost;Database=csharp_demo;Uid=root;Pwd=123456;")
                .UseAutoSyncStructure(false) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                .Build();
            fsql.Aop.CurdAfter += (s, e) =>
            {
                _testOutputHelper.WriteLine(e.Sql);
                foreach (var eDbParm in e.DbParms)
                {
                    if (eDbParm.Value != null) _testOutputHelper.WriteLine(eDbParm.Value.ToString());
                }
            };
            return fsql;
        }
    }
}