using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ParallelLoops;

public class ParallelLoopResult
{
    static int[] arr = Enumerable.Range(0, 100).ToArray();
    static void Main_old(string[] args)
    {
        // Parallel Loops always return ParallelLoopResult

        // Break() is very similar to Stop(), but they are different in their own ways.
        // It is similar in terms of its Proactive comparing with exceptions.
        // Stop tries to stop all the new iteration from start.
        // while Break(), suppose at iteration 65, all the iterations higher/later than 65,
        // parallel loop will try to stop any new iteration, however if the Iteration is lower it will
        // keep on working.

        int sum = 0;
        Object lockSum = new object();

        System.Threading.Tasks.ParallelLoopResult result;



            result = Parallel.For(0, arr.Length, (index, state) =>
            {
                lock (lockSum)
                {

                    if (!state.ShouldExitCurrentIteration || state.LowestBreakIteration >= index)
                    {
                        if (index == 65)
                            //throw new InvalidOperationException("This is on purpose");
                            state.Break();
                        sum += arr[index];
                        Console.WriteLine(
                            $"Current task id: {Task.CurrentId}; Is thread pool thread: {Thread.CurrentThread.IsThreadPoolThread}"
                        );
                    }
                }
            }
            );
        Console.WriteLine(sum);
    }


        
    
}

