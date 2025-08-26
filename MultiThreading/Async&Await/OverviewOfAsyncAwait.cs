using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiThreading.Async_Await;

public class OverviewOfAsyncAwait
{
    static void Main_old(string[] args)
    {
        // Async & Await
        /*
         * 1: Everything after the await keyword is considered a continuation. When program
                runs to the await keyword, the calling thread is immediately released, so that it is
                free to do any other things.
         * 2: Returns the value
         * 3: Throws exceptions of the task if there is any
         * 4: Manages the synchronization context.
         */

        OutputFirstPokemonAsync();


        #region taskContinuation
        //task.ContinueWith(t =>
        //{
        //    var result = t.Result;

        //    var doc = JsonDocument.Parse(result);
        //    JsonElement root = doc.RootElement;
        //    JsonElement results = root.GetProperty("results");

        //    JsonElement firstPokemon = results[0];

        //    Console.WriteLine($"First Pokemon name: {firstPokemon.GetProperty("name")}");
        //    Console.WriteLine($"First Pokemon url: {firstPokemon.GetProperty("url")}");

        //    Console.WriteLine(firstPokemon.ToString());
        //});
        #endregion

        Console.WriteLine("Main Thread not blocked");
        Console.ReadLine();
    }

    public static async void OutputFirstPokemonAsync()
    {
        using var client = new HttpClient();
        var taskGetPokemonList = client.GetStringAsync("https://pokeapi.co/api/v2/pokemon");

        var result = await taskGetPokemonList;

        var doc = JsonDocument.Parse(result);
        JsonElement root = doc.RootElement;
        JsonElement results = root.GetProperty("results");

        JsonElement firstPokemon = results[0];

        Console.WriteLine($"First Pokemon name: {firstPokemon.GetProperty("name")}");
        Console.WriteLine($"First Pokemon url: {firstPokemon.GetProperty("url")}");

    }
}

/*
 * What happens at await?

When OutputFirstPokemon() hits await taskGetPokemonList;, it checks whether the task has already completed:

If completed, it just continues running synchronously on the same thread.

If not completed (most cases when calling a web API), the compiler splits your method into two parts:

Before the await: Starts the HTTP call and returns control to the caller (Main thread).

After the await: This code is stored as a continuation callback to be executed later.

The thread is released – that means the current thread (in your case, the main thread) is now free to do other work:

Console.WriteLine("Main Thread not blocked");


When the HTTP request finishes, the Task completes. The continuation (the rest of the method) is scheduled to run.

Great follow-up. The magic is mostly compiler-generated code. Here’s what happens conceptually when you write this:

```csharp
var result = await taskGetPokemonList;
```

### 1. Compiler transforms `await`

* `await` **does not block**. Instead, the compiler rewrites your method into a **state machine** behind the scenes.
* Your method is split into:

  1. Code **before** the await.
  2. Code **after** the await (stored as a **continuation**).
* The `await` expression is turned into calls like:

  ```csharp
  if (!taskGetPokemonList.IsCompleted)
  {
      // Register a continuation to resume later
      taskGetPokemonList.GetAwaiter()
                        .OnCompleted(ContinuationMethod);
      return; // exits the async method for now
  }
  ```
* This means control goes back to the caller while the task keeps running.

---

### 2. How does it get the result later?

* When the task finishes, the .NET runtime **invokes that continuation**. Inside that continuation, the compiler inserts:

  ```csharp
  var result = taskGetPokemonList.GetAwaiter().GetResult();
  ```
* `GetResult()`:

  * Returns the **value** of the task if it succeeded (`string` in your example).
  * Throws the **original exception** if the task faulted.
  * If the task was canceled, it throws `TaskCanceledException`.

So `await` is really **syntax sugar** for:

1. *Pause here, come back when the task is done*.
2. *When done, call `GetResult()` to unwrap the value or exception.*

---

### 3. Important details

* **No polling:** The runtime doesn’t keep checking if it’s done. The task signals completion and triggers the callback.
* **Thread choice:** In a console app, the continuation usually runs on a ThreadPool thread. In a UI app, it resumes on the captured synchronization context (UI thread) unless `ConfigureAwait(false)` is used.

 */