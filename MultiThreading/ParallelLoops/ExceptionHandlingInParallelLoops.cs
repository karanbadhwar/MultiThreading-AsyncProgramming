using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ParallelLoops
{
    internal class ExceptionHandlingInParallelLoops
    {
        static int[] arr = Enumerable.Range(0, 100).ToArray();

        static void Main_old(string[] args)
        {
            // Like in Parallel Loops Threads are not created by us.
            // So, we don't have much control over it.
            // So, suppose Exception happens at a particular Partition in a particular thread,
            // parallel loops tries to no longer go to the next iteration.

            /*
             * The loop will run some iterations before and sometimes even a few after 65, depending on thread scheduling.

            Once the exception occurs, no new partitions will be scheduled.

            You’ll get an AggregateException containing your InvalidOperationException.
            */

            int sum = 0;
            Object lockSum = new object();
            int counter = 0;

            try
            {
                Parallel.For(
                    0,
                    arr.Length,
                    (index, state) =>
                    {
                        lock (lockSum)
                        {
                            counter++;
                            if (!state.IsExceptional)
                            {
                                if (index == 65)
                                    throw new InvalidOperationException("This is on purpose");
                                sum += arr[index];
                                Console.WriteLine(
                                    $"Current task id: {Task.CurrentId}; Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}"
                                );
                                Console.WriteLine(counter);
                            }
                        }
                    }
                );
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e.Message);
            }

            //Parallel.For(0, arr.Length + 4, index =>
            //{
            //if (index >= arr.Length) // index will go beyond 99
            //    throw new IndexOutOfRangeException($"Index {index} out of bounds!");

            //    lock (lockSum)
            //    {
            //        sum += arr[index]; // using array to trigger the check
            //    }
            //});

            Console.WriteLine(sum);
        }
    }
}
