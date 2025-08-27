using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.PLINQ;

internal class CancellationInPLINQ
{
    static void Main_old(string[] args)
    {
        using var cts = new CancellationTokenSource();

        var items = Enumerable.Range(1, 20);

        //Cancellation works almost same way as in parallel loops as some threads already been scheduled,
        // so some part will still be executed even if we have called for Cancel.

        var evenNums = items.AsParallel()
            .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
            .WithCancellation(cts.Token)
            .Where(x =>
            {
                //if(x == 8)
                //{
                //    cts.Cancel();
                //}
                //if (x == 5)
                //{
                //    throw new InvalidOperationException("This is intentional at " + 5);
                //}
                //if (x == 20)
                //{
                //    throw new ArgumentNullException("This is intentional at " + 20);
                //}
                Console.WriteLine(
                       $"Processing Number: {x}, Thread Id: {Thread.CurrentThread.ManagedThreadId}"
                   );
                //Console.WriteLine($"{Task.CurrentId}");
                return (x % 2 == 0);
            });



        try
        {
            evenNums.ForAll(item =>
            {
                if (item > 8)
                {
                    cts.Cancel();
                }
                Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            });
        }
        catch (AggregateException ex)
        {
            ex.Handle(x =>
            {
                Console.WriteLine("Exception: " + x.Message);
                return true;
            });
        }
        catch(OperationCanceledException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}