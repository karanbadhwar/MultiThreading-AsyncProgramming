
namespace MultiThreading.Basics;

// Simulation Of Web Server
public class Assignment1
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
                Thread processingThread = new Thread(() => ProcessInput(input));
                processingThread.Start();
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
