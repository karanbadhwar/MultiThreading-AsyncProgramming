using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Async_Await
{
    public class BasicSyntax
    {
        static async Task Main_old(string[] args)
        {
            Console.WriteLine("Starting to do work");
            //await WorkAsync();
            string data = await FetchDataAsync();
            Console.WriteLine("Data is fetched: "+data);
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        public static async Task WorkAsync()
        {
            await Task.Delay(2000);
            // Used await to make the rest of the part of method Continuation.
            Console.WriteLine("Work is done");
        }

        public static async Task<string> FetchDataAsync()
        {
            await Task.Delay(2000);
            return "Complex data";
        }
    }
}
