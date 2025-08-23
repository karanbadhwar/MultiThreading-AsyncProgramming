using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MultiThreading.MultiThreading_MISC.ThreadPoolDemo;

public class Assignment1WithThreadPool
{
    private static Queue<string?> requestsQueue = new Queue<string?>(); // InputQueue
    static void Main_old(string[] args)
    {

        Thread monitoringThread = new Thread(MonitorQueue);
        monitoringThread.Start();

        // Enqueue the requests
        Console.WriteLine("Server is Running. Type 'Exit' to stop.");

        while (true)
        {
            string? cusInput = Console.ReadLine();
            if (cusInput.ToLower() == "exit")
            {
                break;
            }
            requestsQueue.Enqueue(cusInput);
        }

    }

    public static void MonitorQueue()
    {
        while (true)
        {
            if (requestsQueue.Count > 0)
            {
                string? input = requestsQueue.Dequeue();
                //Thread processingThread = new Thread(() => ProcessInput(input));
                //processingThread.Start();

                ThreadPool.QueueUserWorkItem(ProcessInput, input);
            }
            Thread.Sleep(100);
        }
    }
    public static void ProcessInput(object? input)
    {

        Thread.Sleep(2000);
        Console.WriteLine($"Processed input: {input}. Is Thread Pool Thread: {Thread.CurrentThread.IsThreadPoolThread}");
    }
}
