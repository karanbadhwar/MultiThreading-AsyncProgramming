using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ThreadSynchronization;

public class Assignment2WithMonitorLock
{
    private static Queue<string?> requestsQueue = new Queue<string?>(); // InputQueue
    private static Object ticketsLock = new object();

    private static int availableTickets = 10;

    static void Main_old(string[] args)
    {
        Thread monitoringThread = new Thread(MonitorQueue);
        monitoringThread.Start();

        // Enqueue the requests
        Console.WriteLine(
            "Server is Running. \r\n Type 'b' to book a ticket. "
                + "\r\nType 'c' to Cancel the ticket.\r\n"
                + "\r\nType 'Exit' to stop.\r\n"
        );

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
                Thread processingThread = new Thread(() => ProcessBooking(input));
                processingThread.Start();
            }
            Thread.Sleep(100);
        }
    }

    //When a thread holds a lock and then sleeps, the lock is still with that thread. Sleeping does not release the lock.
    //Other threads must wait until that thread calls Monitor.Exit.
    public static void ProcessBooking(string? input)
    {
        if (Monitor.TryEnter(ticketsLock, 2000))
        {
            try
            {
                Thread.Sleep(10000);
                //When Thread A gets the lock (Monitor.Enter succeeds), it owns the lock until Monitor.Exit is called.
                //Inside that block, Thread A goes to sleep for 10 seconds.
                //While sleeping, it still owns the lock.
                if (input == "b")
                {
                    if (availableTickets > 0)
                    {
                        availableTickets--;
                        Console.WriteLine();
                        Console.WriteLine(
                            $"Your seat is booked. {availableTickets} seats are still available"
                        );
                    }
                    else
                    {
                        Console.WriteLine("No Ticket seats is available");
                    }
                }
                else if (input == "c")
                {
                    if (availableTickets < 10)
                    {
                        availableTickets++;
                        Console.WriteLine();
                        Console.WriteLine(
                            $"Your seat is Canceled. {availableTickets} seats are still available"
                        );
                    }
                    else
                    {
                        Console.WriteLine(
                            "Error: Cannot process the ticket cancellation at this time."
                        );
                    }
                }
            }
            finally
            {
                Monitor.Exit(ticketsLock);
            }
        }
        else
        {
            Console.WriteLine("The System is busy.");
        }
    }
}
