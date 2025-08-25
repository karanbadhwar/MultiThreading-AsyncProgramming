using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming
{
    public class TaskUsesThreadPool
    {
        static void Main_old(string[] args)
        {
            Task task = Task.Run(Work);
            // Task uses ThreaPool thread by default.
            Console.ReadLine();
        }
        public static void Work()
        {
            Console.WriteLine("I love programming!");
            Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread);
        }
    }
}
