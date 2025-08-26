using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ParallelLoops
{
    internal class BreakMethod
    {
        static int[] arr = Enumerable.Range(0, 100).ToArray();
        static void Main_old(string[] args)
        {
            // Break() is very similar to Stop(), but they are different in their own ways.
            // It is similar in terms of its Proactive comparing with exceptions.
            // Stop tries to stop all the new iteration from start.
            // while Break(), suppose at iteration 65, all the iterations higher/later than 65,
            // parallel loop will try to stop any new iteration, however if the Iteration is lower it will
            // keep on working.

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
                        if (!state.ShouldExitCurrentIteration || state.LowestBreakIteration >= index)
                        {
                            if (index == 65)
                                //throw new InvalidOperationException("This is on purpose");
                                state.Break();
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

/*
 * What Break() actually serves

Break() does not instantly stop all threads.

Parallel.For splits your range into partitions (chunks of indices).

When one partition calls Break(), it updates LowestBreakIteration and tells the scheduler:

“No need to start any new work beyond this index.”

But any work already in progress on other threads will finish.

Work already started can include higher indexes.

If a thread picked up indices 80–99 before thread 1 hit index 65 and called Break(), that partition will still run to completion because it’s already scheduled.

That’s why you see 85, 86, 87, … even though you broke at 65.

So what is the point of Break()?

It prevents further scheduling of chunks beyond the break index.

If your loop body is expensive, this can save time because no new work beyond the break point will start once the break is noticed.

It’s often used when you’ve found what you needed (like searching sorted data or calculating something only up to a condition).
 */