using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiThreading.Async_Await;

public class ContinuationAfterReturnValue
{
    static async Task Main_old(string[] args)
    {
        using var client = new HttpClient();
       
        var PokemonListJson = await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");

        // Get the first Pokemon's URL.
        var doc = JsonDocument.Parse(PokemonListJson);
        JsonElement root = doc.RootElement;
        JsonElement results = root.GetProperty("results");

        JsonElement firstPokemon = results[0];

        var url = firstPokemon.GetProperty("url").ToString();

        // Get the detailed JSON
        var firstPokemonDetailsJson = await client.GetStringAsync(url);

        // PArsing weight and Height
        doc = JsonDocument.Parse(firstPokemonDetailsJson);
        root = doc.RootElement;
        Console.WriteLine($"Name: {root.GetProperty("name").ToString()}");
        Console.WriteLine($"Weight: {root.GetProperty("weight").ToString()}");
        Console.WriteLine($"Height: {root.GetProperty("height").ToString()}");


        Console.WriteLine("This is the end of the program");
        Console.ReadLine(); // needed otherwise the tasks will not be able to use the console
    }
}

/*
 * await doesn’t care if you are in Main or a background thread. It only cares about the Task being awaited:

Incomplete Task → release the current thread.

Completed Task → continue synchronously on the same thread.
*/