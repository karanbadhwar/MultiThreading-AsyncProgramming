using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.MultiThreading_MISC
{
    public class MakingThreadWait
    {
        static void Main_old(string[] args)
        {
            //Thread.SpinWait(10); // It is just like Looping (10) times. It will be kicked out only coz of Time slicing.
            // If taking way too long to work, that's why keep the number of Iterations less.
            //Think of Thread.SpinWait as a tiny "do nothing" loop where the CPU keeps running instructions but doesn’t leave the current thread.
            //Thread.SpinWait(int iterations) just executes a CPU instruction repeatedly for the specified number of iterations.
            //There’s no collection, no list, no condition—it’s just a tight internal loop to waste time on purpose.

            //Thread.Sleep(100); // this will make Thread scheduler kick the Thread out of the CPU.
            // As the threads state changes from Running to WaitSleepJoin

            //SpinWait.SpinUntil();

            Console.WriteLine("Start spinning...");
            Thread.SpinWait(5000000); // CPU spins in a loop for a short time
            Console.WriteLine("Done spinning");
        }
    }
}
