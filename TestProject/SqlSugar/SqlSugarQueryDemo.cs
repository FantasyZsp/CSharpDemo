using Newtonsoft.Json;
using SqlSugar;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.SqlSugar
{
    public class SqlSugarQueryDemo
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SqlSugarQueryDemo(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private SqlSugarClient CreateLocalSqlSugar()
        {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = @"Server=localhost;Database=csharp_demo;Uid=root;Pwd=123456;", //连接符字串
                DbType = DbType.MySql, //数据库类型
                IsAutoCloseConnection = true //不设成true要手动close
            });
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                foreach (var sugarParameter in pars)
                {
                    _testOutputHelper.WriteLine(sugarParameter.Value?.ToString());
                }

                _testOutputHelper.WriteLine(sql); //输出sql,查看执行sql
            };

            return db;
        }

        [Fact]
        public async void Test_Join_SimpleClient()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var simpleClient = sqlSugar.GetSimpleClient<CustomerSugar>();
            // var customer = new CustomerSugar();
            // var card = new CardSugar();


            var sugars = await simpleClient.Context.Queryable<CustomerSugar>()
                .InnerJoin<CardSugar>((cus, card) => cus.CardId == card.CardId)
                .Where((cus, card) => cus.CardId == 1)
                .Select((cus, card) => new
                {
                    CardId = cus.CardId,
                    CardNo = card.CardNo,
                    Name = cus.Name
                })
                .OrderBy(cus => cus.Name)
                .ToPageListAsync(0, 10);


            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(sugars));
        }

        [Fact]
        public async void Test_SelectForUpdate()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var simpleClient = sqlSugar.GetSimpleClient<CustomerSugar>();
            // var customer = new CustomerSugar();
            // var card = new CardSugar();

            var sugars = await simpleClient.AsQueryable()
                .Where(cus => cus.Name == "Test_InsertBatch-1")
                .With("for update")
                .SingleAsync();

            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(sugars));
        }
    }
}