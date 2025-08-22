using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ThreadSynchronization;

// When we use the exclusive lock, generally that creates a Lock of type Monitor Lock.
// But Monitor itself gives more control.
// Just like its Name, it monitors the Critical Section that no more than one Thread can Enter
    // the critical section.

public class MonitorLock
{
    private static int counter = 0;
    private static readonly object lockObject = new object();
    static void Main_old(string[] args)
    {
        {
            Thread t1 = new Thread(IncrementCounter);
            t1.Start();
            //t1.Join();

            Thread t2 = new Thread(IncrementCounter);
            t2.Start();


            t1.Join();
            t2.Join();
            Console.WriteLine($"Final COunter: {counter}");
        }
    }

    public static void IncrementCounter()
    {
        for (int i = 0; i < 100000; i++)
        {
            // Monitor Lock
            // Critical Section
            Monitor.Enter(lockObject);
            try
            {
                counter++;
            }
            finally
            {
                Monitor.Exit(lockObject);
            }

            // Exclusive Lock Creates Monitor Lock internally.
            // Compiler when compiles converts the Exclusive lock to Monitor lock.
            // Exclusive Lock has Try,catch and finally inside it. to make the part Atomic.
        }
    }
}
