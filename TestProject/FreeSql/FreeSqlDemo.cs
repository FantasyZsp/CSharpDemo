using System;
using System.Linq;
using FreeSql;
using FreeSql.Internal;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestProject.FreeSql
{
    public class FreeSqlDemo
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public FreeSqlDemo(ITestOutputHelper testOutputHelper)
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

        [Fact]
        public void Test()
        {
            var freeSql = CreateLocalFreeSql();
            var customer = new Customer
            {
                Id = null,
                Name = "testInsertOrUpdateUni",
                Email = "testEmail4",
                CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };

            var executeAffrows = freeSql.InsertOrUpdate<Customer>()
                .SetSource(customer)
                .UpdateColumns(new[] {"Email"})
                .ExecuteAffrows();


            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(customer));
            _testOutputHelper.WriteLine(executeAffrows.ToString());
        }

        /// <summary>
        ///  TODO insert 如何传参
        /// </summary>
        [Fact]
        public void Test_Sql()
        {
            var freeSql = CreateLocalFreeSql();
            var customer = new Customer
            {
                Id = null,
                Name = "testInsertOrUpdateUni",
                Email = "testEmail4",
                CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var executeAffrows = freeSql.Ado.ExecuteNonQuery("INSERT INTO `customer`( `Name`, `Email`, `CardId`, `Mark`, `Config`) VALUES( '@Name', '@Email', 1, Mark, 'Config')ON DUPLICATE KEY UPDATE `Email` = VALUES(`Email`) ",
                new
                {
                    Name = "testInsertOrUpdateUni",
                    Email = "testEmail4",
                    CardId = 1004,
                    Mark = "mark4",
                    Config = "config4"
                });
            _testOutputHelper.WriteLine(executeAffrows.ToString());
        }


        [Fact]
        public void Test_Update_OnColumn()
        {
            var freeSql = CreateLocalFreeSql();
            var customer = new Customer
            {
                Id = 1,
                Name = "1",
                Email = "testEmail4",
                CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var executeAffrows = freeSql
                .Update<Customer>()
                .Set(c => c.Email, customer.Email)
                .Set(c => c.Name, customer.Name + "1")
                // .Set(c => customer.Config)
                .Where(c => c.Id == customer.Id)
                .ExecuteAffrows();

            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(customer));
            _testOutputHelper.WriteLine(executeAffrows.ToString());
        }

        [Fact]
        public void Test_Update_WhenNotNullAndWhereNotOnlyId()
        {
            var freeSql = CreateLocalFreeSql();
            var customer = new Customer
            {
                Id = 1,
                Name = "1",
                Email = "testEmail4",
                // CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var executeAffrows = freeSql
                .Update<Customer>()
                .SetSourceIgnore(customer, col => col == null)
                // .Set(c => customer.Config)
                .Where(c => c.Name == customer.Name) // 这里在id的前提下追加条件更新，customer给出的值必须有id
                .ExecuteAffrows();

            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(customer));
            _testOutputHelper.WriteLine(executeAffrows.ToString());
        }

        [Fact]
        public void Test_Update_AllAndNotNullAndWhereId()
        {
            var freeSql = CreateLocalFreeSql();
            var customer = new Customer
            {
                Id = 1,
                Name = "1",
                // Email = "testEmail4",
                CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var executeAffrows = freeSql
                .Update<Customer>()
                .SetSourceIgnore(customer, col => col == null)
                .ExecuteAffrows();
            _testOutputHelper.WriteLine(executeAffrows.ToString());
        }

        [Fact]
        public void Test_Update_SetColumnAndOnlyWhereColumnExceptId()
        {
            var freeSql = CreateLocalFreeSql();
            var customer = new Customer
            {
                Id = 1,
                Name = "1",
                Email = "testEmail4",
                // CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var executeAffrows = freeSql
                .Update<Customer>()
                .Set(c => c.Email, customer.Email)
                .Set(c => c.Name, customer.Name + "1")
                // .Set(c => customer.Config)
                .Where(c => c.Name == customer.Name)
                .ExecuteAffrows();
            _testOutputHelper.WriteLine(executeAffrows.ToString());
        }

        [Fact]
        public void Test_Update_ColumnAndOnlyWhereColumnExceptId()
        {
            var freeSql = CreateLocalFreeSql();
            var customer = new
            {
                Id = 1,
                Name = "11",
                Email = (string) null, // 也会置为null
                // CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var executeAffrows = freeSql
                .Update<Customer>()
                .SetDto(customer)
                .Where(c => c.Name == customer.Name)
                .ExecuteAffrows();
            _testOutputHelper.WriteLine(executeAffrows.ToString());
        }


        [Fact]
        public void Test_Update_Sql_ColumnAndOnlyWhereColumnExceptId()
        {
            var freeSql = CreateLocalFreeSql();
            var customer = new
            {
                Id = 1,
                Name = "11",
                Email = "customerEmail", // 也会置为null
                // CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var executeAffrows = freeSql
                .Update<Customer>()
                .SetRaw("Email = ?Email,Mark = ?Mark", customer)
                .Where(c => c.Name == customer.Name)
                .ExecuteAffrows();
            _testOutputHelper.WriteLine(executeAffrows.ToString());
        }

        [Fact]
        public void Test_Insert()
        {
            var freeSql = CreateLocalFreeSql();
            var customer = new Customer
            {
                Id = null,
                Name = "222-33",
                Email = "testEmail4",
                CardId = 1004,
                Mark = "mark4",
                Config = "config4"
            };
            var executeAffrows = freeSql
                .Insert(customer)
                .ExecuteIdentity();

            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(customer));
            _testOutputHelper.WriteLine(executeAffrows.ToString());
        }

        public class Customer
        {
            public uint? Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public int? CardId { get; set; }
            public string Mark { get; set; }
            public string Config { get; set; }
        }
    }
}