using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * No, Thread.Sleep does not release a lock.

When a thread acquires a lock (e.g., using lock in C# or Monitor.Enter), 
that lock is held until the thread exits the synchronized block.
 */

namespace MultiThreading.ThreadSynchronization
{
    public class NestedLock_Deadlocks
    {
        private static Object userLock = new object();
        private static Object orderLock = new object();
        static void Main_old(string[] args)
        {
            Thread t1 = new Thread(ManageUser);
            t1.Start();

            Thread t2 = new Thread(ManageOrder);
            t2.Start();
        }
        public static void ManageUser()
        {
            lock(userLock)
            {
                Console.WriteLine("user Management acquired the user lock.");
                Thread.Sleep(2000);

                lock(orderLock)
                {
                    Console.WriteLine("User management acquired Order lock");
                }
            }
        }

        public static void ManageOrder()
        {
            lock (orderLock)
            {
                Console.WriteLine("user Management acquired the order lock.");
                Thread.Sleep(1000);

                lock (userLock)
                {
                    Console.WriteLine("User management acquired user lock");
                }
            }
        }
    }
}
