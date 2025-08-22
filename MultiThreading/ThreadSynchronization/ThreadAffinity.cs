using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ThreadSynchronization;

public class ThreadAffinity
{
    static void Main_old(string[] args)
    {
        //Thread affinity means that a particular piece of work or a resource is tied to a specific thread,
        //and it must always be accessed or executed by that same thread.
        // but Semaphore does not have Thread Affinity, a thread can squire the semaphore,
        // and another thread can release it.

        // Suppose we created a UI button in Main thread and we try to change it or access it
        // thru different thread an exception can be thrown.
        // As Different thread owns that Button!!, so in order to synchronized both the threads
        // we need to run Invoke() to merge them for request or synchronization of Context.
        //Example: In WPF/WinForms, you use Dispatcher.Invoke() or Control.Invoke() to run code on the UI thread.
    }
}
