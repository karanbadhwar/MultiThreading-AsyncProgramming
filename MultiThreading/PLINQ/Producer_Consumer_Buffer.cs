using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.PLINQ;

public class Producer_Consumer_Buffer
{
    static void Main_old(string[] args)
    {
        var items = Enumerable.Range(1, 200);

        // As LINQ operations are Deferred, one task here is producing and the other is
        // consuming. But they have Buffer as when the termination operation is hit,
        // PLINQ first fills up the Buffer and then Consumer in this case, termination operation,
        // starts consuming.

        //## Most of the options we can do on PLINQ, only after AsParallel()!!!

        //ParallelMergeOptions only work with ForEach not with ForALL!!

        // Producer
        var evenNums = items
            .AsParallel()
            .WithMergeOptions(ParallelMergeOptions.AutoBuffered)
            .Where(x =>
            {
                Console.WriteLine(
                    $"Processing Number: {x}, Thread Id: {Thread.CurrentThread.ManagedThreadId}"
                );
                //Console.WriteLine($"{Task.CurrentId}");
                return (x % 2 == 0);
            });


        // Consumer
        foreach( var item in evenNums )
        {
            Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        }
        //evenNums.ForAll(item =>
        //{
        //    Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        //});
    }
}
