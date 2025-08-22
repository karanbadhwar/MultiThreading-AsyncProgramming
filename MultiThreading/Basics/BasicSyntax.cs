
namespace MultiThreading.Basics;

public class BasicSyntax
{
    static void Main_old(string[] args)
    {
        Console.WriteLine("Basic Syntax");
        //BasicSyntax bs = new BasicSyntax();
        //bs.writeThreadId();
        
        Thread thread1 = new Thread(writeThreadId);
        Thread thread2 = new Thread(writeThreadId);

        thread1.Name = "Thread1";
        thread2.Name = "Thread2";
        Thread.CurrentThread.Name = "Main Thread";

        thread1.Priority = ThreadPriority.Highest;
        thread2.Priority = ThreadPriority.Lowest;
        Thread.CurrentThread.Priority = ThreadPriority.Normal;

        thread2.Start();
        thread1.Start();
        BasicSyntax.writeThreadId();
    }


    public static void writeThreadId()
    {
        for (int i = 0; i < 100; i++)
        {
            Console.WriteLine(Thread.CurrentThread.Name);
        }
    }
}
