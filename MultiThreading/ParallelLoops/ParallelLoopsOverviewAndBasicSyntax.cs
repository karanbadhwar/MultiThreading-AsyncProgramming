using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ParallelLoops;

public class ParallelLoopsOverviewAndBasicSyntax
{
    static int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    static void Main_old(string[] args)
    {
        // Parallel Loop:- The other aspect of Concurrent Programming.
        // As async-await helps us with the async side which is offload the long running task,
        // where as the Parallel loops helps us with the Divide and Conquer side of parallelism in programming.
        // We use parallel loops to run asynchronously but written like Synchronous code.
        int sum = 0;
        Object lockSum = new object();

        // Sync Code
        //for (int i = 0; i < arr.Length; i++)
        //{
        //    sum += arr[i];
        //}

        // Async Code, feels like sync
        //Parallel.For(0, arr.Length, index =>
        //{
        //    lock(lockSum)
        //    {
        //    sum += arr[index];
        //    }
        //});

        Parallel.ForEach(arr, item =>
        {
            lock(lockSum)
            {
                sum += item;
            }
        });

        // Invoke takes in array of delegates.
        Parallel.Invoke(
            () =>
            {
                Console.WriteLine("I am one");
            },
            () =>
            {
                Console.WriteLine("I am Two");
            }
            );

        Console.WriteLine("The sum is: "+sum);
        Console.ReadLine();
    }
}
