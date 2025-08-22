namespace MultiThreading.ThreadSynchronization;

// Example for Race Condition -> when Threads are interfering with each other, because
    // they are trying to sue the same shared resources.
public class Basics
{
    public static int counter = 0;
    static void Main_old(string[] args)
    {
        Thread t1 = new Thread(IncrementCounter);
        t1.Start();
        //t1.Join();

        Thread t2 = new Thread(IncrementCounter);
        t2.Start();


        t1.Join();
        t2.Join();
        Console.WriteLine($"Final COunter: {counter}");
    }

    public static void IncrementCounter()
    {
        for (int i = 0; i < 100000; i++)
        {
            // Critical Section
            counter++;
            // Behind  this 3 steps happen
            /*
             * var temp = counter;
             * temp = temp + 1;
             * counter = temp;
             */
        }
    }
}
