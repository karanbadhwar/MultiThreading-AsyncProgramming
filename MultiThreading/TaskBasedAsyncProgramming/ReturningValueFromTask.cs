using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming
{
    public class ReturningValueFromTask
    {
        static void Main_old(string[] args)
        {
            Task<int> task = Task.Run(Work);
            // No need shared resources to return from the task.
            int result = task.Result;

            Console.WriteLine($"The result is {result}");
            Console.ReadLine();
        }

        public static int Work()
        {
            Console.WriteLine("I love programming!");
            Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread);

            int result = 100;
            return result;
        }
    }
}
