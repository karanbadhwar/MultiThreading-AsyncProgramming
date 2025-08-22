using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ThreadSynchronization.AutoResetEven_Signaling;

public class SignalingAutoResetEvent
{
    public static AutoResetEvent AutoResetEvent = new AutoResetEvent(false);
    static void Main_old(string[] args)
    {
        Console.WriteLine("Server is running, type 'go' to proceed");

        string? userInput = null;

        //Thread workerThread = new Thread(Worker);
        //workerThread.Start();

        for(int i = 0; i < 3; i++)
        {
            Thread workerThread = new Thread(Worker);
            workerThread.Name = $"Worker {i + 1}";
            workerThread.Start();
        }

        while(true)
        {
            userInput = Console.ReadLine()??"";

            // Signal the worker thread if the input is "go"
            if(userInput.ToLower() == "go")
            {
                AutoResetEvent.Set();
            }
            if(userInput.ToLower() == "e")
            {
                break;
            }
        }

        //workerThread.Join();
        //AutoResetEvent.Dispose();
    }

    public static void Worker()
    {
        while (true)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for the signal");

            AutoResetEvent.WaitOne();

            Console.WriteLine($"{Thread.CurrentThread.Name} Thread proceeds");

            Thread.Sleep(2000);
        }
    }
}
