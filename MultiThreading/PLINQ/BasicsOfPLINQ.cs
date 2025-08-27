using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.PLINQ;

public class BasicsOfPLINQ
{
    static void Main_old(string[] args)
    {
        // PLINQ -> Parallel LINQ

        var items = Enumerable.Range(1, 20);

        // so under the hood multiple threads will be used to get the result.
        // Same as parallel loops, it will divide the collection into Chunks and then take diff.
        // threads to get the result.
        // Here we added AsOrdered to just order out the sequence as different thread can work on diff value.
        var evenNums = items
            .AsParallel()
            .AsOrdered()
            .Where(x =>
            {
                Console.WriteLine(
                    $"Processing Number: {x}, Thread Id: {Thread.CurrentThread.ManagedThreadId}"
                );
                //Console.WriteLine($"{Task.CurrentId}");
                return (x % 2 == 0);
            });

        //var evenNums = items.Where(x =>
        //{
        //    //Console.WriteLine($"Processing Number: {x}");
        //    return (x % 2 == 0); // This is still normal, suppose we have complex processing
        //});

        //Console.WriteLine(evenNums.Count());

        //foreach( var item in evenNums )
        //{
        //    Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        //}

        evenNums.ForAll(item =>
        {
            Console.WriteLine($"{item}: Thread Id: {Thread.CurrentThread.ManagedThreadId}");
        });

        //Console.ReadLine();
    }
}

/*
 * ForAll does not wait for the entire PLINQ query to finish first.

It works like a pipeline:

PLINQ starts processing the query in parallel, splitting data into chunks and filtering it.

As soon as an item passes the Where filter, it is immediately handed over to ForAll to run your action (e.g., print).

This means query execution and output happen concurrently: one item is being filtered while another is already being printed.

But your main thread (the one that called ForAll) blocks until all items are done. Once all worker tasks finish, ForAll returns.
 */
