using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ParallelLoops;

internal class CancelationInParallelLoop
{
    static void Main_old(string[] args)
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        Task task = Task.Run(() => Work(token), token);

        Console.WriteLine("To cancel, press 'c'");
        var input = Console.ReadLine();
        if (input == "c")
        {
            cts.Cancel();
        }
        task.Wait();

        Console.WriteLine("Task status after wait(): " + task.Status);
    }

    private static void Work(CancellationToken token)
    {
        Console.WriteLine("Started doing work");
        //for (int i = 0; i < 100000; i++)
        //{
        //    Console.WriteLine($"{DateTime.Now}");
        //    if (token.IsCancellationRequested)
        //    {
        //        Console.WriteLine("User requested cancellation at iteration: " + i);
        //        //break;
        //        token.ThrowIfCancellationRequested();
        //    }
        //    Thread.SpinWait(3000000);
        //}
        ParallelOptions options = new ParallelOptions { CancellationToken = token };

        try
        {
            Parallel.For(0, 100000, options, index =>
            {
                // Exception is automatically thrown when cancellation token is triggered.
                Console.WriteLine($"{DateTime.Now}");
                Thread.SpinWait(30000000);
            });
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Parallel loop canceled via token.");
        }
        catch (AggregateException ex)
        {
            foreach (var inner in ex.InnerExceptions)
                Console.WriteLine("Parallel exception: " + inner.Message);
        }

        Console.WriteLine("Work is Done");
    }
}
