using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.SqlSugar
{
    public class SqlSugarDemo
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public SqlSugarDemo(ITestOutputHelper testOutputHelper)
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
                // foreach (var sugarParameter in pars)
                // {
                //     _testOutputHelper.WriteLine(sugarParameter.Value?.ToString());
                // }

                _testOutputHelper.WriteLine(sql); //输出sql,查看执行sql
            };

            return db;
        }


        [Fact]
        public void Test_SugarInsertOrUpdateStringId()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var now = DateTime.UtcNow;
            var executeAffRows = sqlSugar.Ado.ExecuteCommand(
                @"insert into student(stuId, name, email, createTime, updateTime) 
                    VALUES (@stuId,@name,@email,@createTime,@updateTime) 
                    on duplicate key update email = values(email),name = values(name)",
                new
                {
                    stuId = $"1511e57db97045dc90ecd14ca3880b15",
                    name = "testxxx",
                    email = "xxx",
                    createTime = now,
                    updateTime = now
                });
            _testOutputHelper.WriteLine(executeAffRows.ToString());
        }

        [Fact]
        public void Test_UpdateBatch()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var customers = sqlSugar.Queryable<CustomerSugar>()
                .Where(customer => customer.Email == "testEmail4xxx")
                .ToList();

            foreach (var customer in customers)
            {
                customer.Email += "xxx";
                customer.CardId += customer.CardId;
            }

            var executeAffRows = sqlSugar
                .Updateable(customers)
                .UpdateColumns(customer => new {customer.Email})
                .ExecuteCommand();
            _testOutputHelper.WriteLine(executeAffRows.ToString());
        }

        [Fact]
        public void Test_UpdateBatchById()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var names = new List<string> {"testInsertOrUpdate", "testInsertOrUpdate2", "testInsertOrUpdate3"};
            var affRows = sqlSugar.Updateable(names.Select(name => new CustomerSugar {Name = name, Mark = "UpdateBatch"}).ToList())
                .UpdateColumns(task => new {task.Mark})
                // .Where(task => names.Contains(task.Name))
                .ExecuteCommand();
            _testOutputHelper.WriteLine(affRows.ToString());
        }

        [Fact]
        public void Test_UpdateAllColumnById_SimpleClient()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var simpleClient = sqlSugar.GetSimpleClient<CustomerSugar>();
            var customer = new CustomerSugar
            {
                Name = "testInsertOrUpdate3", Mark = "Test_UpdateBatchById_Repository", CardId = 11, Config = "config", Email = "email"
            };

            var affRows = simpleClient.Update(customer); // customer属性不能有null,是因为对sql的监听

            _testOutputHelper.WriteLine(affRows.ToString());
        }

        [Fact]
        public void Test_UpdateColumnsByWhere_SimpleClient()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var simpleClient = sqlSugar.GetSimpleClient<CustomerSugar>();
            var customer = new CustomerSugar
            {
                Name = "testInsertOrUpdate3", Mark = "Test_UpdateBatchById_Repository", CardId = 11, Config = "config", Email = "email"
            };

            var affRows = simpleClient.Update(cc => new CustomerSugar
            {
                Mark = cc.Mark
            }, cc => cc.CardId == customer.CardId && cc.Name == customer.Name);

            _testOutputHelper.WriteLine(affRows.ToString());
        }

        [Fact]
        public void Test_UpdateNonNullColumnsByWhere_SimpleClient()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var simpleClient = sqlSugar.GetSimpleClient<CustomerSugar>();
            var customer = new CustomerSugar
            {
                Name = "Test_UpdateNonNullColumnsByWhere_SimpleClient", Mark = "2333", Config = "xxx"
            };
            var affRows = simpleClient.AsUpdateable(customer)
                .IgnoreColumns(true)
                .Where(cc => cc.Mark == customer.Mark)
                .ExecuteCommand();

            _testOutputHelper.WriteLine(affRows.ToString());
        }

        [Fact]
        public async void Test_Cas_SimpleClient()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var simpleClient = sqlSugar.GetSimpleClient<CustomerSugar>();
            var customer = new CustomerSugar
            {
                Name = "Test_Cas_SimpleClient", Mark = "33333333", Config = "44444"
            };
            var affRows = await simpleClient.AsUpdateable(customer)
                .UpdateColumns(customerSugar => new {customerSugar.Mark, customerSugar.Config})
                .Where(customerSugar => customerSugar.Mark == "2333")
                .ExecuteCommandAsync();

            _testOutputHelper.WriteLine(affRows.ToString());
        }

        [Fact]
        public async void Test_Insert()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var simpleClient = sqlSugar.GetSimpleClient<CardSugar>();
            var cardSugar = new CardSugar
            {
                CardId = 1230001, Mark = "33333333", CardNo = "44444"
            };
            var affRows = await simpleClient.Context.Insertable(cardSugar).ExecuteReturnEntityAsync();
            // affRows = await simpleClient.AsInsertable(cardSugar)
            //     .ExecuteReturnEntityAsync();

            _testOutputHelper.WriteLine(affRows.ToString());
        }
    }
}