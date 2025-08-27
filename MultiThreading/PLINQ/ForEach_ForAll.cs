using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.PLINQ;

public class ForEach_ForAll
{
    static void Main_old(string[] args)
    {
        var items = Enumerable.Range(1, 20);

        //ParallelMergeOptions only work with ForEach not with ForALL!!

        // Producer
        var evenNums = items
            .AsParallel()
            .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
            .Where(x =>
            {
                Console.WriteLine(
                    $"Processing Number: {x}, Thread Id: {Thread.CurrentThread.ManagedThreadId}"
                );
                //Console.WriteLine($"{Task.CurrentId}");
                return (x % 2 == 0);
            });

        // Consumer
        //foreach (var item in evenNums)
        //{
        //    Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        //}

        evenNums.ForAll(item =>
        {
            Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        });
    }
}
