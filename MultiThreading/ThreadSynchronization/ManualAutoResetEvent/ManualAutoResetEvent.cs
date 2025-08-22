namespace MultiThreading.ThreadSynchronization.ManualAutoResetEvent
{
    public class ManualAutoResetEvent
    {
        public static ManualResetEventSlim manualResetEvent = new ManualResetEventSlim(false);
        static void Main_old(string[] args)
        {
            Console.WriteLine("Press enter to release all threads");
            for (int i = 0; i < 3; i++)
            {
                Thread thread = new Thread(work);
                thread.Name = $"Thread {i + 1}";
                thread.Start();
            }

            
            Console.ReadLine();

            manualResetEvent.Set();
        }

        private static void work(object? obj)
        {
            Console.WriteLine($"{Thread.CurrentThread.Name} is waiting for the signal...");
            manualResetEvent.Wait();
            //manualResetEvent.Reset(); // it will reset the signal to OFF once one thread comes here
            Thread.Sleep(2000);
            Console.WriteLine($"{Thread.CurrentThread.Name} has been released");
        }
    }
}
