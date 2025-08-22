using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ThreadSynchronization;

public class ExclusiveLock
{

    private static Object counterLock = new Object(); // in C# 12 and .net 8
    //System.Threading.Lock counterLock = new Object(); // in C# 13 and .net 9, somewhat this
    
    public static int counter = 0;
    static void Main_old(string[] args)
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

    public static void IncrementCounter()
    {
        for (int i = 0; i < 100000; i++)
        {
            // Critical Section
            lock (counterLock) // Exclusive Lock
            {
                counter++;
            }
            // Behind  this 3 steps happen
            /*
             * var temp = counter;
             * temp = temp + 1;
             * counter = temp;
             * Thread 1: read counter (say 5)
                Thread 2: read counter (also 5)
                Thread 1: write 6
                Thread 2: write 6

             */

            // Exclusive Lock has Try,catch and finally inside it. to make the part Atomic.
        }
    }
}
