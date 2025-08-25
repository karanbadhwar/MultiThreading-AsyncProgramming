using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.MultiThreading_MISC
{
    public class ExceptionHandlingInThreads
    {
        public static List<Exception> exceptions = [];
        public static Object objLock = new Object();
        static void Main_old(string[] args)
        {
            // Each Thread has it own Call Stack, so every Exception do Bubbles up.
            // but we cannot catch the exception in the Calling Thread!!
            //    Thread thread = null;
            //    try
            //    {

            //    thread = new Thread(() =>
            //    {
            //        try
            //        {
            //        throw new InvalidOperationException("An Error occurred in this worker thread. This is expected.");
            //        } catch(Exception e)
            //        {
            //            Console.WriteLine("Error Occured");
            //        }
            //    });
            //    } catch(Exception e)
            //    {
            //        Console.WriteLine(e.ToString()+"From Main Thread"); // Even if it throws an exception it will never come to this point.
            //    }
            //    thread?.Start();
            //    thread?.Join();
            //}

            // More better approach
            

            Thread t1 = new Thread(Work);
            Thread t2 = new Thread(Work);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            foreach (Exception ex in exceptions)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void Work()
        {
            try
            {
            throw new InvalidOperationException("An Error occurred in this worker thread. This is expected.");

            } catch (Exception e)
            {
                lock (objLock)
                {
                    exceptions.Add(e);
                }
            }
        }
    }
}
