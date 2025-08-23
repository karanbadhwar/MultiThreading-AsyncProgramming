using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.MultiThreading_MISC;

public class ThreadPoolDemoClass
{
    static void Main_old(string[] args)
    {
        // Creating a new Thread and then disposing it takes a hit at Performance.
        // It takes time, memory and performance to do all that.
        // Thread Pool -> Is a mechanism to reuse the created one threads again.
        // There will be few pre-created Threads and then we can use them to again over and over.
        //in modern .NET (Core, 5, 6, 7, 8) and even in .NET Framework, every Thread you create is backed by an OS-level thread (a “native thread”).
        //Thread pool threads are also OS threads; they’re just reused instead of created each time.

        //Yes, exactly. In .NET, the Thread Pool is created and managed automatically by the CLR (Common Language Runtime) when your application starts. You don’t have to create it yourself.

        /*
         * If you explicitly create a thread in .NET like this:

            var t = new Thread(() =>
            {
                Console.WriteLine("Custom thread");
            });
            t.Start();

            That thread does NOT go to the thread pool.

            Here’s what happens:
            When you call new Thread(...), the CLR asks the OS to create a completely new OS thread just for your delegate.
         */

        ThreadPool.GetMaxThreads(out var maxWorkerThread, out var maxIOThreads);

        Console.WriteLine($"Max Worker Thread is: {maxWorkerThread} & max Io Threads: {maxIOThreads}");

        ThreadPool.GetAvailableThreads(out var availableWorkerThread, out var availableIOThreads);

        Console.WriteLine($"Active Worker Thread is: {maxWorkerThread - availableWorkerThread} & Active Io Threads: {maxIOThreads - availableIOThreads}");

        //ThreadPool.QueueUserWorkItem(a,b); a -> delegate, b -> if we need to pass some info to the delegate
    }
}
