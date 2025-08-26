using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ParallelLoops
{
    internal class StopMethod
    {
        static int[] arr = Enumerable.Range(0, 100).ToArray();

        static void Main_old(string[] args)
        {
            // Stop method works just like Exceptions but here we know, we expect an Exception coming.
            // Generally we use Stop when Each iteration is taking Long Time.
            // While exceptions are more prioritized (Reactive), while stop is more Proactive.
            int sum = 0;
            Object lockSum = new object();
            int counter = 0;

            try
            {
                Parallel.For(0, arr.Length, (index, state) =>
                    {
                        lock (lockSum)
                        {
                            counter++;
                            if (!state.IsStopped)
                            {
                                if (index == 65)
                                    //throw new InvalidOperationException("This is on purpose");
                                    state.Stop();
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


            Console.WriteLine(sum);
        }
    }
}
