using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiThreading.Async_Await
{
    internal class WhatDoesAwaitDo
    {
        static async Task Main(string[] args)
        {
            // So when a compiler sees Async - Await keyword, it creates a State Machine.
            // A State Machine is a special class that can be created to navigate between different states.

            // Example:-
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
            Console.ReadLine();
        }
    }
}
