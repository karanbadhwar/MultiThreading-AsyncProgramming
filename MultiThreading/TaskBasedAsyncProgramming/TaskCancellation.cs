using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming;

public class TaskCancellation
{
    
    static void Main_old(string[] args)
    {
        using var cts = new CancellationTokenSource();
        var token = cts.Token;

        // We can provide the Timeout as well for the token to auto cancel the call.
        cts.CancelAfter(1000);

        Task task = Task.Run(() => Work(token), token);

        Console.WriteLine("To cancel, press 'c'");
        var input = Console.ReadLine();
        if (input == "c")
        {
            cts.Cancel();
        }
        Console.WriteLine("Task status Before wait(): "+task.Status);
        task.Wait();

        Console.WriteLine("Task status after wait(): " + task.Status);
    }

    private static void Work(CancellationToken token)
    {
        Console.WriteLine("Started doing work");
        for (int i = 0; i < 100000; i++)
        {
            Console.WriteLine($"{DateTime.Now}");
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("User requested cancellation at iteration: " + i);
                //break;
                token.ThrowIfCancellationRequested();
            }
            Thread.SpinWait(3000000);
        }
        Console.WriteLine("Work is Done");
    }
}
