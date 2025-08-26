using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ParallelLoops
{
    internal class BehindTheScene
    {
        //static int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        static int[] arr = Enumerable.Range(0,100).ToArray();

        static void Main_old(string[] args)
        {
            // Behind the Scene in Parallel Loops
            //1: Data Partition.
            //2: Mostly use thread pool thread.
            //3: Make the best decision by itself.
            //4: Blocking call.


            //--------------------------------------------------
            // So behind the scene it partitions the data to determine how many threads are needed.
            // SO it divides the collection into specific chunks and that depends on the CPU Core and CPU Processors or different factors.
            // so Microsoft, tried to make the best decision.

            // It uses Thread pool or create a new thread that completely depends on the implementation,
            // Microsoft have chosen the right path.
            // It makes the best decision by itself.
            // Parallel Loops are blocking!!! even though it uses Threads underneath but it waits untill all the tasks are done.
            int sum = 0;
            Object lockSum = new Object();
            Parallel.For(0, arr.Length, index =>
            {
                lock (lockSum)
                {
                    sum += arr[index];
                    Console.WriteLine($"Current task id: {Task.CurrentId}; Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}");
                    //Console.WriteLine($"Current Thread id: {Thread.CurrentThread.ManagedThreadId} ");
                }
            });

            Console.WriteLine("The sum is: " + sum);
            Console.ReadLine();
        }
    }
}
