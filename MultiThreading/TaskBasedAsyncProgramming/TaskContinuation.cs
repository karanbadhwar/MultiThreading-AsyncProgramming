using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming
{
    public class TaskContinuation
    {
        static void Main_old(string[] args)
        {
            // Task Continuation -> As Task returns a Promise, so to hold the Thread at the point,
            // to make the Task Synchronous so we dont skip the Result from Task.
            // We use the Following Methods:
            // 1 - Wait()
            // 2 - WaitAll()
            // 3 - Result()
            // 4 - ContinueWith()
            // 5 - WhenAll()
            // 6 - WhenAny()
            // 7 - Continuation Chain
            // 8 - UnWrap

            // 1 -> Using Wait(), very similar to Thread.Join()
            

            var task = new Task<int>(() =>
            {
                int sum = 0;
                for (int i = 0; i <= 100; i++)
                {
                    Task.Delay(100); // Very Similar to THread.Sleep()
                    sum += i;
                }
                return sum;
            });

            task.Start();

            // task.Wait(); //Just like Thread.Join() blocks the calling Thread for the Task/Thread to finish first

            // Task.WaitAll(task1, task2) // It can even take in array of Tasks, This is
            // for to wait for all the provided tasks to finish.

            // task.Result; // It is same like task.wait()
             var result = task.Result; // It blocks the current Thread, until the result is out
            //task.ContinueWith(t => { }); // Example in Pokemon API

            // Continuation Chain & UnWrap in the Pokemon API.

            Console.WriteLine($"The result is: {result}");

        }
    }
}
