using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;
using TestProject.FreeSql;
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
                foreach (var sugarParameter in pars)
                {
                    _testOutputHelper.WriteLine(sugarParameter.Value.ToString());
                }

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


        // /// <summary>
        // ///  TODO insert 如何传参
        // /// </summary>
        // [Fact]
        // public void Test_Sql()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new Customer
        //     {
        //         Id = null,
        //         Name = "testInsertOrUpdateUni",
        //         Email = "testEmail4",
        //         CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar.Ado.ExecuteNonQuery("INSERT INTO `customer`( `Name`, `Email`, `CardId`, `Mark`, `Config`) VALUES( '@Name', '@Email', 1, Mark, 'Config')ON DUPLICATE KEY UPDATE `Email` = VALUES(`Email`) ",
        //         new
        //         {
        //             Name = "testInsertOrUpdateUni",
        //             Email = "testEmail4",
        //             CardId = 1004,
        //             Mark = "mark4",
        //             Config = "config4"
        //         });
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        //
        // [Fact]
        // public void Test_Update_OnColumnAndWhere()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new Customer
        //     {
        //         Id = 1,
        //         Name = "1",
        //         Email = "testEmail4",
        //         CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar
        //         .Update<Customer>()
        //         .Set(c => c.Email, customer.Email)
        //         .Set(c => c.Name, customer.Name + "1")
        //         // .Set(c => customer.Config)
        //         .Where(c => c.Name == customer.Name)
        //         .ExecuteAffrows();
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        // [Fact]
        // public void Test_Update_OnColumnAndWhere_SetIfWhenTrue()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new Customer
        //     {
        //         Id = 1,
        //         Name = "1",
        //         Email = "testEmail4",
        //         CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar
        //         .Update<Customer>()
        //         .SetIf(customer.Email != null, c => c.Email, customer.Email)
        //         .Set(c => c.Name, customer.Name + "1")
        //         // .Set(c => customer.Config)
        //         .Where(c => c.Name == customer.Name)
        //         .ExecuteAffrows();
        //
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        // [Fact]
        // public void Test_Update_SetWhenNotNullAndWhereAppendWithId()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new Customer
        //     {
        //         Id = 1,
        //         Name = "1",
        //         Email = "testEmail4",
        //         // CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar
        //         .Update<Customer>()
        //         .SetSourceIgnore(customer, col => col == null)
        //         // .Set(c => customer.Config)
        //         .Where(c => c.Name == customer.Name) // 这里在id的前提下追加条件更新，customer给出的值必须有id
        //         .ExecuteAffrows();
        //
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        // [Fact]
        // public void Test_Update_AllAndNotNullAndWhereId()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new Customer
        //     {
        //         Id = 1,
        //         Name = "1",
        //         // Email = "testEmail4",
        //         CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar
        //         .Update<Customer>()
        //         .SetSourceIgnore(customer, col => col == null)
        //         .ExecuteAffrows();
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        // [Fact]
        // public void Test_Update_SetColumnAndOnlyWhereColumnExceptId()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new Customer
        //     {
        //         Id = 1,
        //         Name = "1",
        //         Email = "testEmail4",
        //         // CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar
        //         .Update<Customer>()
        //         .Set(c => c.Email, customer.Email)
        //         .Set(c => c.Name, customer.Name + "1")
        //         // .Set(c => customer.Config)
        //         .Where(c => c.Name == customer.Name)
        //         .ExecuteAffrows();
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        // [Fact]
        // public void Test_Update_SetColumnAndOnlyWhereColumnExceptId2()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new Customer
        //     {
        //         Id = 1,
        //         Name = "1",
        //         Email = "testEmail4",
        //         CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar
        //         .Update<Customer>()
        //         .Set(c => new Customer
        //         {
        //             Name = "1",
        //             Email = "testEmail4",
        //             CardId = null,
        //             Mark = "mark4",
        //             Config = "config4"
        //         })
        //         .Where(c => c.Name == customer.Name)
        //         .ExecuteAffrows();
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        // [Fact]
        // public void Test_Update_ColumnAndOnlyWhereColumnExceptId()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new
        //     {
        //         Id = 1,
        //         Name = "11",
        //         Email = (string) null, // 也会置为null
        //         // CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar
        //         .Update<Customer>()
        //         .SetDto(customer)
        //         .Where(c => c.Name == customer.Name)
        //         .ExecuteAffrows();
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        //
        // [Fact]
        // public void Test_Update_Sql_ColumnAndOnlyWhereColumnExceptId()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new
        //     {
        //         Id = 1,
        //         Name = "11",
        //         Email = "customerEmail", // 也会置为null
        //         // CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar
        //         .Update<Customer>()
        //         .SetRaw("Email = ?Email,Mark = ?Mark", customer)
        //         .Where(c => c.Name == customer.Name)
        //         .ExecuteAffrows();
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        // [Fact]
        // public void Test_Insert()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new Customer
        //     {
        //         Id = null,
        //         Name = "222-33",
        //         Email = "testEmail4",
        //         CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var executeAffrows = sqlSugar
        //         .Insert(customer)
        //         .ExecuteIdentity();
        //
        //     _testOutputHelper.WriteLine(executeAffrows.ToString());
        // }
        //
        // [Fact]
        // public void Test_InsertBatch()
        // {
        //     var sqlSugar = CreateLocalSqlSugar();
        //     var customer = new Customer
        //     {
        //         Id = null,
        //         Name = "Test_InsertBatch-5",
        //         Email = "testEmail4",
        //         CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //
        //     var customer2 = new Customer
        //     {
        //         Id = null,
        //         Name = "Test_InsertBatch-6",
        //         Email = "testEmail4",
        //         CardId = 1004,
        //         Mark = "mark4",
        //         Config = "config4"
        //     };
        //     var list = new List<Customer>() {customer, customer2};
        //
        //     var executeAffRows = sqlSugar
        //         .Insert(list)
        //         .ExecuteIdentity();
        //
        //     _testOutputHelper.WriteLine(JsonConvert.SerializeObject(list));
        //     _testOutputHelper.WriteLine(executeAffRows.ToString());
        // }
        //
        //
        [Fact]
        public void Test_UpdateBatch()
        {
            var sqlSugar = CreateLocalSqlSugar();
            var customers = sqlSugar.Queryable<Customer>()
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
            var affRows = sqlSugar.Updateable(names.Select(name => new Customer {Name = name, Mark = "UpdateBatch"}).ToList())
                .UpdateColumns(task => new {task.Mark})
                // .Where(task => names.Contains(task.Name))
                .ExecuteCommand();
            _testOutputHelper.WriteLine(affRows.ToString());
        }
    }
}