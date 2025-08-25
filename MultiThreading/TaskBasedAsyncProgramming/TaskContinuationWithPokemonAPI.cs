using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming
{
    public class TaskContinuationWithPokemonAPI
    {
        static void Main_old(string[] args)
        {
            using var client = new HttpClient();
            var task = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");

            //var result = task.Result; // This blocks the Main Thread
            //Console.WriteLine(result);

            // Instead use ContinueWith, it receives the previous Task in the params
            // ContinueWith Chains task but depends upon the completion of previous tasks.
            task.ContinueWith(t =>
            {
                var result = t.Result; // It is blocking but not the Main Thread
                //Console.WriteLine(result);

                var doc = JsonDocument.Parse(result);
                JsonElement root = doc.RootElement;
                JsonElement results = root.GetProperty("results");

                JsonElement firstPokemon = results[0];

                Console.WriteLine($"First Pokemon name: {firstPokemon.GetProperty("name")}");
                Console.WriteLine($"First Pokemon url: {firstPokemon.GetProperty("url")}");

                Console.WriteLine(firstPokemon.ToString());
            });

            Console.WriteLine("Main Thread not blocked");
            Console.ReadLine(); // needed otherwise the tasks will not be able to use the console
        }
    }
}

/*
 * Ah, now I get your question. You are asking:
**“In that `HttpClient.GetStringAsync` code, I never created a `Task` or a `Thread`. So where exactly does the work leave the Main thread and who decides to run it somewhere else?”**

Let’s break this down step by step in very clear language.

---

### 1. What happens when you call `GetStringAsync`?

When you do:

```csharp
var task = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");
```

* The method **immediately returns a `Task<string>`**, but the network request is not completed yet.
* Internally, `HttpClient` uses lower-level .NET classes (`SocketsHttpHandler`, sockets, etc.) to talk to the OS networking system.

**Key point:** The Main thread does the setup but **doesn’t sit and wait**. The request is handed off to the operating system.

---

### 2. How does the request actually work if no thread is waiting?

* When you ask for data from the internet, the heavy lifting (sending/receiving packets) is handled by the **network card and OS kernel**, not .NET.
* The OS has a mechanism called **I/O Completion Ports (IOCP)** (on Windows) or similar async I/O on Linux/macOS.
* IOCP is event-based: it says, *“Tell me when the data is ready; I’ll notify you later.”*

This means:

* **No .NET worker thread is waiting.** The Main thread is free to do other work.

---

### 3. Where does the thread come back in?

* When the OS finishes downloading the response, it raises a “completed” event.
* .NET receives this event and schedules your task to finish.
* **At this point**, .NET uses a **ThreadPool thread** to run the continuation (your `task.Result` or your `await`).

This is the moment where the work “leaves” your Main thread and continues on a background thread.

---

### 4. Why you didn’t create a `Task` manually

* `GetStringAsync` is already written as an **async method** by Microsoft.
* Inside it, the library authors call lower-level async socket APIs and wrap them in a `Task`.
* So **you didn’t create the Task**, but **they did inside the method** and returned it to you.

Think of it as: *you ordered food, but the chef is cooking in the kitchen you can’t see.*

---

### 5. Simple analogy

* **Main thread:** “Please download that Pokémon data.”
* **HttpClient:** “Okay, I’ve sent the order to the OS. I’ll let you know when it’s ready.” (returns `Task` immediately)
* **OS:** “I’ll call you when the data is here.”
* **When ready:** .NET takes the event, uses a ThreadPool thread to resume your code and set the Task as completed.

---

### So the answer to “where is the threaded approach used?”

1. **At the start**: Main thread sends the request.
2. **While waiting**: No thread is used; OS handles it.
3. **When data arrives**: .NET picks a **ThreadPool thread** to execute the rest of the code.

The separation from the Main thread happens **inside `HttpClient` and the .NET networking stack**, where they internally create and return a `Task` for you.

---

Is Task “event-based”?

Not exactly, but you can think of it that way:

A Task is basically a promise to deliver a result in the future.

Internally, it works by registering callbacks (continuations) to run when the work is done.

When the operation (network, calculation, etc.) finishes, the Task marks itself as “completed” and triggers those callbacks.
 */

/*
 * How ContinueWith works

ContinueWith says: “When this task finishes, run this other piece of code.”

It registers a continuation callback. When the first task completes, the continuation is queued to run (usually on a ThreadPool thread).

So your Main thread is not blocked; it prints "Main Thread not blocked" and then waits at ReadLine.

How await works

await is built on top of the same idea, but it’s:

Cleaner (less callback nesting).

Context-aware (it can return to the original synchronization context, like a UI thread).

Handles exceptions better (propagates them naturally).

Example:

var result = await client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");
Console.WriteLine(result);


Behind the scenes, await splits your method at the await point. Everything after it becomes a continuation (similar to ContinueWith) but much easier to write and read.
*/