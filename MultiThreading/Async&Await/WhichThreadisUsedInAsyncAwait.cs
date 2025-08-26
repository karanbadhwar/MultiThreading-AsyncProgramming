using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Async_Await;

public class WhichThreadisUsedInAsyncAwait
{
    static async Task Main_old(string[] args)
    {
        Console.WriteLine($" 1. Main thread id: {Thread.CurrentThread.ManagedThreadId}");

        Console.WriteLine("Starting to do work");
        //await WorkAsync();
        string data = await FetchDataAsync();
        //await DoWork(); // this does not await as nothing async has been returned from Method.
        Console.WriteLine("Data is fetched: " + data);

        Console.WriteLine($"2. Thread id after the AWAIT keyword: {Thread.CurrentThread.ManagedThreadId}");

        Console.WriteLine("Press enter to exit");
        Console.ReadLine();
    }

    //public static async Task WorkAsync()
    //{
    //    await Task.Delay(2000);
    //    // Used await to make the rest of the part of method Continuation.
    //    Console.WriteLine("Work is done");
    //}

    public static async Task<string> FetchDataAsync()
    {
        Console.WriteLine($"3. Thread id before the AWAIT Keyword: {Thread.CurrentThread.ManagedThreadId}");
        await Task.Delay(2000);
        Console.WriteLine(Thread.CurrentThread.IsThreadPoolThread);
        Console.WriteLine($"4. Thread id after the AWAIT keyword: {Thread.CurrentThread.ManagedThreadId}");
        return "Complex data";
    }

    public static async Task DoWork()
    {
        Console.WriteLine("Started");
        
        Console.WriteLine("Finished");
    }

}

/*
 * Main() [Thread 1]
   |
   --> FetchDataAsync() runs synchronously until await [Thread 1]
          |
          Console.WriteLine("3...") [Thread 1]
          |
          await Task.Delay(2000)
               |
               - Returns control to Main()
               - Timer runs for 2 seconds
               - Continuation queued to ThreadPool
   |
   --> Resumes after Task.Delay [ThreadPool Thread, e.g., Thread 5]
          |
          Console.WriteLine("4...") [Thread 5]
          return "Complex data"

*/