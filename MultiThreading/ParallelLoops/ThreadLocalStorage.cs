using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ParallelLoops;

public class ThreadLocalStorage
{
    static int[] arr = Enumerable.Range(0,1000000).ToArray();
    static void Main_old(string[] args)
    {
        int sum =0;
        Object lockSum = new Object();
        DateTime beginTime = DateTime.Now;

        // approx 21 milisecs, not every situation is best for Multi-threading.
        // As Creating a thread, context switching takes time...
        // It does depends on iterations, time the task is taking and so on.
        Parallel.For(
            0, // Starting point
            arr.Length, // ending point
            () => 0, // local state for individual thread, Initialized
            (index, state, tls) => // Func to perform operation
            {
                tls += arr[index];
                return tls;
            },
            tls => // finally block to aggregate the local variable to total sum
            {
                lock (lockSum) // now Locking iteration gets less, than going over 1000000 times
                {
                    sum += tls;
                    Console.WriteLine($"The task id: {Task.CurrentId}");
                }
            }
            );

        // approx 10 milisecs
        //for (int i = 0; i < arr.Length; i++)
        //{
        //    sum += arr[i];
        //}
        DateTime endTime = DateTime.Now;

        Console.WriteLine($"The sum is {sum}");

        Console.WriteLine($"Time spent: {(endTime-beginTime).TotalMilliseconds}");

        Console.ReadLine();
    }
}
