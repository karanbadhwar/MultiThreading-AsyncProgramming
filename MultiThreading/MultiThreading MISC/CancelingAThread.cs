using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.MultiThreading_MISC
{
    public class CancelingAThread
    {
        public static bool cancelThread = false; // Shared Resource

        static void Main_old(string[] args)
        {
            Thread thread = new Thread(Work);
            thread.Start();

            Console.WriteLine("To cancel, press 'c'");
            var input = Console.ReadLine();
            if (input == "c")
            {
                cancelThread = true;
            }
            thread.Join();


        }

        private static void Work()
        {
            Console.WriteLine("Started doing work");
            for(int i = 0; i < 100000; i++)
            {
                if(cancelThread)
                {
                    Console.WriteLine("User requested cancellation at iteration: "+i);
                    break;
                }
                Thread.SpinWait(30000);
            }
            Console.WriteLine("Work is Done");
        }
    }
}
