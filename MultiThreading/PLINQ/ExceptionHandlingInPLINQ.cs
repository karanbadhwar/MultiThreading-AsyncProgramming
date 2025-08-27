using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.PLINQ;

internal class ExceptionHandlingInPLINQ
{
    static void Main_old(string[] args)
    {
        var items = Enumerable.Range(1, 20);

        //So once the exception is found PLINQ tries to shutdown the process!!!
        //So, if the thread did not start the work at that specific Number or Iteration,
        // that particular exception can be skipped.
        //Remember, Exceptions an be handled in the Consumer part not in the Producer part.

        var evenNums = items.AsParallel()
            .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
            .Where(x =>
            {
                if (x == 5)
                {
                    throw new InvalidOperationException("This is intentional at "+5);
                }
                if (x == 20)
                {
                    throw new ArgumentNullException("This is intentional at "+20);
                }
                Console.WriteLine(
                       $"Processing Number: {x}, Thread Id: {Thread.CurrentThread.ManagedThreadId}"
                   );
                //Console.WriteLine($"{Task.CurrentId}");
                return (x % 2 == 0);
            });


        //foreach (var item in evenNums)
        //{
        //    Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        //}

        try
        {
            evenNums.ForAll(item =>
            {
                Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            });
        }
        catch (AggregateException ex)
        {
            ex.Handle(x =>
            {
                Console.WriteLine("Exception: "+x.Message);
                return true;
            });
        }
    }
}
