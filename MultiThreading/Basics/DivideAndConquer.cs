namespace MultiThreading.Basics;

public class DivideAndConquer
{
    static int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

    static void Main_old(string[] args)
    {
        int sum1 = 0,
            sum2 = 0,
            sum3 = 0,
            sum4 = 0;
        //int sum = 0;
        var startTime = DateTime.Now;

        int numOfThreads = 4;
        int segmentLength = arr.Length / numOfThreads;

        Thread[] threads = new Thread[numOfThreads];
        threads[0] = new Thread(() =>
        {
            sum1 = SumSegment(0, segmentLength);
        });

        threads[1] = new Thread(() =>
        {
            sum2 = SumSegment(segmentLength, 2 * segmentLength);
        });

        threads[2] = new Thread(() =>
        {
            sum3 = SumSegment(2*segmentLength, 3*segmentLength);
        });

        threads[3] = new Thread(() =>
        {
            sum4 = SumSegment(3 * segmentLength, arr.Length);
        });

        foreach (var thread in threads)
        {
            thread.Start();
            //thread.Join();
            /*
             * What happens now:

                thread.Start() → start the first thread.
                thread.Join() → main thread waits until the first thread finishes.
                Only after that does it start the second thread, then immediately joins it, and so on.
                Threads are no longer running concurrently, they run sequentially.
                Total time ≈ sum of all threads’ execution times, which is why it’s much larger.
             */
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }
        //foreach (int i in arr)
        //{
        //    Thread.Sleep(100);
        //    sum += i;
        //}

        var endTime = DateTime.Now;


        Console.WriteLine($"The sum is {sum1+sum2+sum3+sum4}");
        Console.WriteLine($"The time it takes : {(endTime - startTime).TotalMilliseconds}");
    }

    public static int SumSegment(int start, int end)
    {
        int segmentSum = 0;
        for (int i = start; i < end; i++)
        {
            Thread.Sleep(100);
            segmentSum += arr[i];
        }
        return segmentSum;
    }
}
