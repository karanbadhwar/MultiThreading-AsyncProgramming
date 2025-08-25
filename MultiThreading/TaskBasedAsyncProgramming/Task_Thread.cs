using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming;

public class Task_Thread
{
    static void Main_old(string[] args)
    {
        // Task -> Promise, A promise that we are going to get this task done in the future,
        // but not sure when.
        // Task most of the time actually uses thread to perform the task, but it doesn't have to.

        // Thread -> A thread is a basic programming unit for running something in a CPU.

        // Reasons to use Task Over Thread.
        /*
         * uses thread pool by default.
         * returning values, we don't need shared resources
         * Easy Continuation.
         * Better exception handling.
         * Async/Await: makes writing async code feels like writing synchronous code.
         * Async/Await: Easy Context management(Sync context with async and await keyword)
         */

        /*
         * 1. A Task is just a unit of work, not always a thread
        A Task is just a logical unit of work. It doesn’t guarantee a new or dedicated thread.
            Task represents an operation that may run asynchronously.
            It uses the Task Scheduler to decide how and where to run that operation.
            Most of the time, the default TaskScheduler runs tasks on the ThreadPool, but:
            Sometimes it may run inline on the calling thread (e.g., if you call task.RunSynchronously() or if a continuation can execute immediately).
            It can also be scheduled on a custom scheduler (e.g., a UI thread scheduler like WPF’s Dispatcher).

            2. Examples showing different behavior

            Case 1 – Background thread (common case)
            Task task = new Task(Work);
            task.Start();   // runs on a ThreadPool thread (background)


            Case 2 – Inline execution on main thread
            Task task = new Task(Work);
            task.RunSynchronously();  // runs on the same thread (no new thread)


            Case 3 – Custom scheduler (UI thread)
            // In WPF or WinForms, you can post to UI thread's scheduler
            Task.Factory.StartNew(Work, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext());


            This runs on the UI thread, not a ThreadPool thread.
         */
    }
}
