using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Async_Await;

public class ExceptionHandlingInAsyncAwait
{
    static async Task Main_old(string[] args)
    {
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

        // With await Exceptions are not hidden.
        await Task.WhenAll(tasks); // Await makes the exception to be thrown, but it throws
                                    // the first Exception that can be any.
        


        Console.WriteLine("Enter key to exit");

        Console.ReadLine();
    }
}
