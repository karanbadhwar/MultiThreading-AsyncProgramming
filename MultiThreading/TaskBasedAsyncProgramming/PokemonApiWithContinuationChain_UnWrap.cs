using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming
{
    public class PokemonApiWithContinuationChain_UnWrap
    {
        static void Main_old(string[] args)
        {
            using var client = new HttpClient();
            // First Task gets the List JSON
            var taskListJson = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");

            // Next Task is to get the First Pokemon's Info.
            var taskFirstPokemonUrl = taskListJson.ContinueWith(t =>
            {
                var result = t.Result;

                var doc = JsonDocument.Parse(result);
                JsonElement root = doc.RootElement;
                JsonElement results = root.GetProperty("results");

                JsonElement firstPokemon = results[0];

                return firstPokemon.GetProperty("url").ToString();
            });

            // Next Task is to get first Pokemon's more info detailed
            var taskGetDetailJSON = taskFirstPokemonUrl.ContinueWith(t =>
            {
                string result = t.Result;
                return client.GetStringAsync(result);
            }).Unwrap();
            // as It returned Task<Task<string>> but after unwrap it removes extra layer of Task.

            taskGetDetailJSON.ContinueWith(t =>
            {
                var result = t.Result;
                var doc = JsonDocument.Parse(result);
                JsonElement root = doc.RootElement;
                Console.WriteLine($"Name: {root.GetProperty("name").ToString()}");
                Console.WriteLine($"Weight: {root.GetProperty("weight").ToString()}");
                Console.WriteLine($"Height: {root.GetProperty("height").ToString()}");
            });

            Console.WriteLine("Main Thread not blocked");
            Console.ReadLine(); // needed otherwise the tasks will not be able to use the console
        }
    }
}
