using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ThreadSynchronization;

public class Assignment2
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

    public static void ProcessBooking(string? input)
    {
        Thread.Sleep(2000);
        lock (ticketsLock)
        {
            if (input == "b")
            {
                if (availableTickets > 0)
                {
                    availableTickets--;
                    Console.WriteLine();
                    Console.WriteLine(
                        $"Your seat is booked. {availableTickets} seats are still available"
                    );
                } else
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
                    Console.WriteLine("Error: Cannot process the ticket cancellation at this time.");
                }
            }
        }
    }
}
