using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming
{
    public class Assignment4WithWhenAll_WhenAny
    {
        static int[] arr = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        static void Main_old(string[] args)
        {

            var startTime = DateTime.Now;

            int numOfThreads = 4;
            int segmentLength = arr.Length / numOfThreads;

            Task<int>[] tasks = new Task<int>[numOfThreads];
            tasks[0] = Task.Run(() =>
            {
                return SumSegment(0, segmentLength);
            });

            tasks[1] = Task.Run(() =>
            {
                return SumSegment(segmentLength, 2 * segmentLength);
            });

            tasks[2] = Task.Run(() =>
            {
                return SumSegment(2 * segmentLength, 3 * segmentLength);
            });

            tasks[3] = Task.Run(() =>
            {
                return SumSegment(3 * segmentLength, arr.Length);
            });



            //Console.WriteLine($"The sum is {tasks.Sum(t => t.Result)}"); // This is blocking

            // wait for all the tasks to finish, it is an aggregation
            Task.WhenAll(tasks).ContinueWith(t =>
            {
                Console.WriteLine($"The sum is: {t.Result.Sum()}");
            });

            // Don't wait for all the tasks, just any task finishes run the callback registered with continueWith 
            //Task.WhenAny(tasks).ContinueWith(t =>
            //{
            //    Console.WriteLine($"The sum is: {t.Result.Result}");
            //});


            Console.WriteLine("This is the end of program");
            var endTime = DateTime.Now;
            Console.WriteLine($"The time it takes : {(endTime - startTime).TotalMilliseconds}");
            Console.ReadLine();
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
}

