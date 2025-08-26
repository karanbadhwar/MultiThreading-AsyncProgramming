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
        static async Task Main_old(string[] args)
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

/*
 * Great code example! When you mark a method as `async` and use `await`, the C# compiler rewrites the method into a **state machine**. Each `await` represents a possible suspension point, so the compiler generates states to "remember" where execution should continue after the awaited task completes.

In your `Main` method, you have **two `await` calls**:

1. `await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");`
2. `await client.GetStringAsync(url);`

Here’s how the compiler will roughly generate the **states**:

---

### **1. State -1 (Initial)**

* This is before anything runs. It’s the entry point of the state machine.
* The compiler sets the initial state to `-1` and starts executing the method until it hits the first `await`.

### **2. State 0 (Waiting on the first await)**

* Execution reaches `await client.GetStringAsync(...)`.
* The method returns control to the caller (releasing the thread), and the compiler stores the continuation in state `0`.
* When the HTTP call finishes, execution resumes **right after this `await`**.

### **3. State 1 (Waiting on the second await)**

* Execution reaches `await client.GetStringAsync(url)`.
* Again, the method suspends, returns to the caller, and stores the continuation in state `1`.
* When this HTTP call completes, execution resumes **right after this `await`**.

---

### **Final State -2 (Completed)**

* After the second `await` finishes, the method runs to the end and sets the state to `-2`, which means **completed**.

---

So, in total:

* **-1** → Start (before anything happens)
* **0** → After first `await` suspends
* **1** → After second `await` suspends
* **-2** → Done


 */

/*
 * Perfect! Let’s break down how the compiler transforms your `Main` method into a state machine.

When you compile an `async` method with `await`, the compiler rewrites it into a hidden struct/class implementing `IAsyncStateMachine`. The method body is moved into a `MoveNext()` method that uses a `switch` statement to jump between **states**.

Here’s a **simplified version of what the compiler roughly generates** for your `Main` method (not the exact internal names, but conceptually accurate):

```csharp
internal class WhatDoesAwaitDo
{
    private sealed class <Main>d__0 : IAsyncStateMachine
    {
        public int <>1__state;               // Stores the current state (-1, 0, 1, etc.)
        public AsyncTaskMethodBuilder <>t__builder; // Helps manage async flow
        private HttpClient <client>5__1;     // Your variable captured
        private string <PokemonListJson>5__2;
        private string <url>5__3;

        private TaskAwaiter<string> <>u__1;  // Awaiter for first GetStringAsync
        private TaskAwaiter<string> <>u__2;  // Awaiter for second GetStringAsync

        void IAsyncStateMachine.MoveNext()
        {
            int num = <>1__state;
            try
            {
                if (num == -1)
                {
                    // Initial code before first await
                    Console.WriteLine("Program started");
                    <client>5__1 = new HttpClient();

                    var task1 = <client>5__1.GetStringAsync("https://pokeapi.co/api/v2/pokemon");
                    <>u__1 = task1.GetAwaiter();
                    if (!<>u__1.IsCompleted)
                    {
                        <>1__state = 0;           // Save state
                        <>t__builder.AwaitUnsafeOnCompleted(ref <>u__1, ref this);
                        return;                   // Suspend here
                    }
                }
                if (num == 0)
                {
                    // Resume after first await
                    <PokemonListJson>5__2 = <>u__1.GetResult();

                    // Parse JSON and get URL...
                    var doc = JsonDocument.Parse(<PokemonListJson>5__2);
                    var results = doc.RootElement.GetProperty("results");
                    var firstPokemon = results[0];
                    <url>5__3 = firstPokemon.GetProperty("url").ToString();

                    var task2 = <client>5__1.GetStringAsync(<url>5__3);
                    <>u__2 = task2.GetAwaiter();
                    if (!<>u__2.IsCompleted)
                    {
                        <>1__state = 1;           // Save state
                        <>t__builder.AwaitUnsafeOnCompleted(ref <>u__2, ref this);
                        return;                   // Suspend here
                    }
                }
                if (num == 1)
                {
                    // Resume after second await
                    var firstPokemonDetailsJson = <>u__2.GetResult();

                    // Parse and print details...
                    var doc = JsonDocument.Parse(firstPokemonDetailsJson);
                    var root = doc.RootElement;
                    Console.WriteLine($"Name: {root.GetProperty("name")}");
                    Console.WriteLine($"Weight: {root.GetProperty("weight")}");
                    Console.WriteLine($"Height: {root.GetProperty("height")}");

                    Console.WriteLine("This is the end of the program");
                }
            }
            catch (Exception ex)
            {
                <>1__state = -2;
                <>t__builder.SetException(ex);
                return;
            }

            <>1__state = -2;             // Completed
            <>t__builder.SetResult();
        }

        void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine) { }
    }

    static Task Main(string[] args)
    {
        var stateMachine = new <Main>d__0();
        stateMachine.<>t__builder = AsyncTaskMethodBuilder.Create();
        stateMachine.<>1__state = -1;
        stateMachine.<>t__builder.Start(ref stateMachine);
        return stateMachine.<>t__builder.Task;
    }
}
```

### Key points:

* `<>1__state` holds the **state** (`-1` start, `0` after first await, `1` after second await, `-2` completed).
* Each `await` becomes an **awaiter** (`TaskAwaiter<string>`).
* The compiler turns your method into a **switch-like flow** using `if/else` or `switch` to resume where it left off.
* When a task is not yet finished, `AwaitUnsafeOnCompleted` schedules the continuation and returns control.

 */