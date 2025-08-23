using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.MultiThreading_MISC
{
    public class DebuggingThreads
    {
        static void Main_old(string[] args)
        {
            for(int i = 0; i < 10;  i++)
            {
                Thread thread = new Thread(Work);
                thread.Name = $"Thread {i}";
                thread.Start();
            }
            Thread.CurrentThread.Name = "Master Thread";
            Work();
        }

        public static void Work()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.Name} started working");
            Thread.Sleep(10000);
            Console.WriteLine($"Thread {Thread.CurrentThread.Name} finished working");
        }
    }
}
