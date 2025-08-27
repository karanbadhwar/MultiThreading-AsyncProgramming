using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ConcurrentCollections;

public class ConcurrentQueue
{
    /*
    static void Main(string[] args)
    {
// In Concurrent Collection there is already a locking mechanism that implements Thread Synchronization.
        ConcurrentQueue<int> queue = new ConcurrentQueue<int>();

        queue.Enqueue(1);
        queue.Enqueue(2);
        queue.Enqueue(3);

        // There is no normal Dequeue
        queue.TryDequeue(out var result);

        Console.WriteLine(result);
    }
    */
    private static ConcurrentQueue<string> requestsQueue = new ConcurrentQueue<string>(); // InputQueue
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
                if(requestsQueue.TryDequeue(out string? input))
                {
                    Thread processingThread = new Thread(() => ProcessInput(input));
                    processingThread.Start();
                }
            }
            Thread.Sleep(100);
        }
    }

    public static void ProcessInput(string? input)
    {
        Thread.Sleep(2000);
        Console.WriteLine($"Processed input: {input}");
    }
}
