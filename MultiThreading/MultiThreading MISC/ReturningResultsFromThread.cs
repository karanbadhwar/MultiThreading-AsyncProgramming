using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.MultiThreading_MISC
{
    public class ReturningResultsFromThread
    {
        public static string? result;
        static void Main_old(string[] args)
        {
            // It is not possible to return a result from a thread if we are using the thread class.
            // Task Class do have some functionality to return though.
            // But with Thread class there is no functionality to return from a Thread. We can get the result from 
                // a SHARED RESOURCE!!!

            Thread thread = new Thread(Work);
            thread.Start();

            thread.Join();

            Console.WriteLine("The result from Worker thread is: "+result);
        }

        public static void Work()
        {
            Console.WriteLine("Started doing some work");
            Thread.Sleep(1000);

            result = "Here is the result"; // Returning through Shared Resource. // Same way did in Divide & Conquer.
        }
    }
}
