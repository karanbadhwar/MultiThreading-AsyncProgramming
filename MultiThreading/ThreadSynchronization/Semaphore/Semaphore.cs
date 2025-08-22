using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ThreadSynchronization.Semaphore
{
    public class Semaphore
    {
        static void Main_old(string[] args)
        {
            // Semaphore is a Synchronization technique that can help us to limit the number of
                // Concurrent Threads or Processes.
            // Semaphore is like Mutex, if Name is Provided it is used System Wide.

            /* // Means 3 Thread at max can use the Critical Section Simultaneously!!
             * using SemaphoreSlim semaphore = new Semaphore(initialCount:3, maXCOunt:3)
             * { // Everytime a Thread accesses the critical section by using wait(), InitialCount gets - 1
             *  semaphore.wait(); // COunt gets decreased by 1 till 0
             *  try
             *  {
             *      use semaphore to limit access to the following section
             *      // code
             *  }finally{
             *      semaphore.Release(); count again gets increased by 1 till MaxCount
             *  }
             * }
             */
        }
    }
}
