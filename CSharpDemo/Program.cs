using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // StringExample.TestString();
            // MyEnumTest.Test();
            // LinqExample.TestLinq();

            var program = new Program();


            Console.WriteLine($"test {Thread.CurrentThread.ManagedThreadId}");

            // await 
            await program.PrintAsync();
            Console.WriteLine($"after PrintAsync {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(1000);
        }

        public async Task PrintAsync()
        {
            Console.WriteLine($"PrintAsync {Thread.CurrentThread.ManagedThreadId}");

            var sumTask = SumTask();
            // await sumTask;
            Console.WriteLine($" after SumTask {Thread.CurrentThread.ManagedThreadId}");
        }

        public Task SumTask()
        {
            Console.WriteLine($"build SumTask {Thread.CurrentThread.ManagedThreadId}");

            // var run = Task.Run(async () =>
            // {
            //     Console.WriteLine($"SumTaskResult invoking by {Thread.CurrentThread.ManagedThreadId}");
            //     int sum = 0;
            //     for (int i = 0; i < 100000; i++)
            //     {
            //         sum = +i;
            //     }
            //
            //     await File.WriteAllBytesAsync("e.txt", new Byte[10]);
            //
            //     Console.WriteLine($"SumTaskResult {sum} by {Thread.CurrentThread.ManagedThreadId}");
            //     return sum;
            // });

            Console.WriteLine($"SumTaskResult invoking by {Thread.CurrentThread.ManagedThreadId}");
            int sum = 0;
            for (int i = 0; i < 100000; i++)
            {
                sum = +i;
            }

            var task = File.WriteAllBytesAsync("e.txt", new Byte[10]);
            Console.WriteLine($"SumTaskResult {sum} by {Thread.CurrentThread.ManagedThreadId}");
            return task;

            // return run;
        }
    }
}