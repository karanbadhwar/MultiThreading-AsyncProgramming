// Producer - Consumer With manual AutoResetEvent
namespace MultiThreading.ThreadSynchronization.ManualAutoResetEvent
{
    public class Assignment3
    {
        private static Queue<int> queue = new Queue<int>();
        public static ManualResetEventSlim manual = new ManualResetEventSlim(false);
        public static ManualResetEventSlim producerManual = new ManualResetEventSlim(true);
        private static object lockConsumerCount = new object();

        public static int consumerCount = 0;

        static void Main_old(string[] args)
        {

            Thread[] consumerThread = new Thread[3];
            for (int i = 0; i < 3; i++)
            {
                consumerThread[i] = new Thread(Consume);
                consumerThread[i].Name = $"Consumer {i+1}";
                consumerThread[i].Start();
            }

            // Producer Section
            while (true)
            {
                producerManual.Wait();
                producerManual.Reset();

                Console.WriteLine("To Produce enter 'p'");
                var input = Console.ReadLine() ?? "";

                if (input.ToLower() == "p")
                {
                    for (int i = 0; i < 10; i++)
                    {
                        queue.Enqueue(i);
                    }
                    manual.Set();
                    Console.WriteLine("Queue is full now again");

                }
            }
        }

        //Consumer Section
        public static void Consume()
        {
            while (true)
            {
                manual.Wait();
                while (queue.TryDequeue(out int item))
                {
                    // Work on the items produced
                    Thread.Sleep(500);
                    Console.WriteLine($"Consumed: {item} from thread: {Thread.CurrentThread.Name}");
                }

                lock(lockConsumerCount)
                {
                    consumerCount++;
                    manual.Reset();
                    if (consumerCount == 3)
                    {
                        //manual.Reset();
                        Console.WriteLine("Queue is empty now, please produce more!!");
                        producerManual.Set();
                    }
                }

               
            }
        }
    }
}
