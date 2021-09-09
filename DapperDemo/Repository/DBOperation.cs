using System;
using System.Data;
using System.Text;
using System.Threading;
using Dapper;
using DapperDemo.Repository.Entity;
using MySql.Data.MySqlClient;

namespace DapperDemo.Repository
{
    public static class DbOperation
    {
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var con =
                new MySqlConnection(
                    "server=127.0.0.1;database=csharp_demo;uid=root;pwd=123456;charset='utf8mb4';SslMode=None");
            //连接字符串要加上 SslMode=None 不然会报错误：SSL not supported in this WinRT release.
            //新增数据
            con.Open();
            // insertAndReturnIdAndUpdate(con);
            // upsert(con);
            upsertAndReturnId(con);
            queryAndPrintAllCustomer(con);
            // deleteById(con, 1);
            Thread.Sleep(3000);
        }

        private static void insertAndReturnIdAndUpdate(MySqlConnection con)
        {
            con.Execute("insert into customer values(null, 'test', 30)");
            //新增数据返回自增id
            var id = con.QueryFirst<int>(
                "insert into customer(id, name, email) values(null, 'testInsert', 38);select last_insert_id();");
            //修改数据
            con.Execute("update customer set name = 'testUpdate' where `Id` = @Id", new {Id = id});
        }

        private static void deleteById(MySqlConnection con, int id)
        {
            con.Execute("delete from customer where Id = @Id", new {Id = id});
            Console.WriteLine("删除数据后");
            var list = con.Query<Customer>("select * from customer");
            foreach (var item in list)
            {
                Console.WriteLine($"用户名：{item.Name} 年龄：{item.Id}");
            }
        }

        private static void queryAndPrintAllCustomer(IDbConnection con)
        {
            //查询数据
            var list = con.Query<Customer>("select * from customer");
            foreach (var item in list)
            {
                Console.WriteLine($"用户名：{item.Name} id：{item.Id}");
            }
        }

        private static int upsert(IDbConnection con)
        {
            // 新增数据传入参数
            return con.Execute(@"insert into customer( name, email) 
            values( @Name, @Email) on duplicate key update email = 500;",
                new {Name = "testInsertWithParams", Email = 0});
        }

        private static int upsertAndReturnId(IDbConnection con)
        {
            // 新增数据传入参数
            var id = con.QueryFirst<int>(@"insert into customer( name, email) 
            values( @Name, @Email) on duplicate key update email = 500;SELECT id AS Id from customer where name = @Name;",
                new {Name = "testInsertWithParams", Email = 0});
            Console.WriteLine("id to return is " + id);
            return id;
        }
    }
}