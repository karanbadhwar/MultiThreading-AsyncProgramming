using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming;

public class ExceptionHandlingInTask
{
    static void Main_old(string[] args)
    {
        // Exceptions in Tasks are actually Hidden, unlike In Threads.
        // Using Try catch does not work!!
        // Exceptions are stored in the task itself.
        // exceptions -> multiple ones can be stored hence we can iterate them.
        // Using wait() or Result, will make the stored exception thrown.
        /*
        using var client = new HttpClient();
        var task1 = client.GetStringAsync("https://pokeapi123.co/api/v2/pokemon");

        var task2 = task1.ContinueWith(t =>
        {
            var result = t.Result;

            var doc = JsonDocument.Parse(result);
            JsonElement root = doc.RootElement;
            JsonElement results = root.GetProperty("results");

            JsonElement firstPokemon = results[0];

            Console.WriteLine($"First Pokemon name: {firstPokemon.GetProperty("name")}");
            Console.WriteLine($"First Pokemon url: {firstPokemon.GetProperty("url")}");

            Console.WriteLine(firstPokemon.ToString());
        }, TaskContinuationOptions.NotOnFaulted);

        Thread.Sleep(1000);
        //Task.Delay(1000);
        Console.WriteLine(task1.Status);
        Console.WriteLine(task2.Status);

        if (task1.IsFaulted && task1.Exception != null)
        {
            foreach(var ex in task1.Exception.InnerExceptions)
            {
                Console.WriteLine(ex.Message);
            }
        }

        Console.WriteLine("Main Thread not blocked");
        Console.ReadLine();
        */

        var tasks = new[]
        {
            Task.Run(() =>
            {
                throw new InvalidOperationException("Invalid Operation");
            }),
            Task.Run(() =>
            {
                throw new ArgumentNullException("Argument null");
            }),
            Task.Run(() =>
            {
                throw new Exception("General Exception");
            }),
        };

        var taskAll = Task.WhenAll(tasks);
        taskAll.Wait(); // this will make exception thrown


        Console.WriteLine("Enter key to exit");

        Console.ReadLine();
    }
}
