using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming;

internal class TaskSynchronization
{
    private static Queue<string?> requestsQueue = new Queue<string?>(); // InputQueue
    private static Object queueLock = new object();
    static void Main_old(string[] args)
    {
        // Task Synchronization is same as Thread Synchronization.

        using SemaphoreSlim semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3);

        Task monitoringTask = new Task(() => MonitorQueue(semaphore));
        monitoringTask.Start();

        // Enqueue the requests
        Console.WriteLine("Server is Running. Type 'Exit' to stop.");

        while (true)
        {
            string? cusInput = Console.ReadLine();
            if (cusInput.ToLower() == "exit")
            {
                break;
            }
            lock (queueLock)
            {
                requestsQueue.Enqueue(cusInput);
            }
        }

    }

    public static void MonitorQueue(SemaphoreSlim semaphore)
    {
        while (true)
        {
            if (requestsQueue.Count > 0)
            {
                string? input;
                lock (queueLock)
                {
                    input = requestsQueue.Dequeue();
                }
                semaphore.Wait();
                Task processingTask = new Task(() => ProcessInput(input, semaphore));
                processingTask.Start();
            }
            Thread.Sleep(100);

        }
    }

    public static void ProcessInput(string? input, SemaphoreSlim semaphore)
    {
        try
        {
            Thread.Sleep(2000);
            Console.WriteLine($"Processed input: {input}");
        }
        finally
        {
            int prevCount = semaphore.Release();
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} released the semaphore. Previous count is : {prevCount}");
        }

    }
}

