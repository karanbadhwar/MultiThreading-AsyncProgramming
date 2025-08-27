using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ConcurrentCollections;

public class BlockingCollection_Producer_Consumer
{
    //BlockingCOllection is best for Producer and Consumer Pattern...
    //There are a lot more Concurrent Collection!!!
    // We can use Blocking COllection to wrap around the concurrent collections.
    // Related to the Producer & Consumer example, Blocking & Bounding refers to BUFFER.
    // Bounding -> means there is maximum capacity of the buffer, we can specify that.
    // Blocking -> means that when the maximum capacity is reached the producer should stop producing,
    // and wait until consumer consumes products in the buffer.
    // So when there is space in the Buffer, Producer can start producing.
    // Blocking on Upper Bound, when the buffer is full, Producer needs to stop producing.
    // Blocking on Lower Bound, when the buffer is empty, Consumer should stop consuming.

    //static void Main(string[] args)
    //{
    //    // Features of Blocking Collection:-
    //    // 1-> Blocking
    //    // 2-> Bounding
    //}
    private static ConcurrentQueue<string> requestsQueue = new ConcurrentQueue<string>();

    private static BlockingCollection<string> collection = new BlockingCollection<string>(requestsQueue, 3);
    static void Main(string[] args)
    {

        Thread monitoringThread = new Thread(MonitorQueue);
        monitoringThread.Start();

        // Enqueue the requests
        Console.WriteLine("Server is Running. Type 'Exit' to stop.");

        while (true)
        {
            string? cusInput = Console.ReadLine();
            if (cusInput.ToLower() == "exit")
            {
                collection.CompleteAdding();
                break;
            }
            //requestsQueue.Enqueue(cusInput);
            collection.Add(cusInput);
        }

    }

    public static void MonitorQueue()
    {


        foreach (var req in collection.GetConsumingEnumerable())
        {
            if (collection.IsCompleted)
            {
                break;
            }
            Thread processingThread = new Thread(() => ProcessInput(req));
            processingThread.Start();

            Thread.Sleep(100);
        }

        //if (requestsQueue.Count > 0)
        //{
        //    if (requestsQueue.TryDequeue(out string? input))
        //    {
        //        Thread processingThread = new Thread(() => ProcessInput(input));
        //        processingThread.Start();
        //    }
        //}
        //Thread.Sleep(100);

    }

    public static void ProcessInput(string? input)
    {
        Thread.Sleep(2000);
        Console.WriteLine($"Processed input: {input}");
    }
}

/*Good question. `BlockingCollection<T>` is more than just a bounded queue with blocking. It’s basically a **high-level producer-consumer utility** built on top of thread-safe collections like `ConcurrentQueue<T>` or `ConcurrentBag<T>`.

Here’s what it adds beyond "just bounding and blocking":

---

### **1. Producer-Consumer Coordination**

* Provides **blocking** semantics:

  * `Add()` will wait if the collection is full (when bounded).
  * `Take()` will wait if the collection is empty.
* No need to manually use `Thread.Sleep` or poll `Count`.

---

### **2. Multiple Underlying Collections**

* Internally wraps any `IProducerConsumerCollection<T>`:

  * By default, uses `ConcurrentQueue<T>` (FIFO).
  * Can also use `ConcurrentStack<T>` or `ConcurrentBag<T>`.
* This means you get **different ordering semantics** without writing your own logic.

---

### **3. Built-In Cancellation and Completion**

* Supports **cancellation tokens** with `Add()` and `Take()`.
* Supports **CompleteAdding()**:

  * Producers can signal "no more items will be added."
  * Consumers can then finish cleanly without polling forever.

---

### **4. Enumeration Support**

* Can be **enumerated safely** even while items are being added/taken.
* Enumeration automatically **blocks until items are available** and ends when `CompleteAdding()` is called and collection is emptied.

---

### **5. Thread Safety with Simplicity**

* Handles all the synchronization internally (no need for locks, conditions, or semaphores).
* Safe for multiple producers and multiple consumers.

---

### **6. Useful Methods**

* `TryAdd`, `TryTake` with timeouts.
* `GetConsumingEnumerable()` – consumer-friendly enumeration.
* Works well in **Task-based producer-consumer patterns**.

---

### **Example**

```csharp
var bc = new BlockingCollection<int>(boundedCapacity: 5);

// Producer
Task.Run(() =>
{
    for (int i = 0; i < 20; i++)
    {
        bc.Add(i); // waits if full
        Console.WriteLine($"Produced {i}");
    }
    bc.CompleteAdding();
});

// Consumer
foreach (var item in bc.GetConsumingEnumerable())
{
    Console.WriteLine($"Consumed {item}");
}
```

* Consumers will block if no items are available.
* Producers will block if the capacity is full.

---

### **When to Use**

* Best for **producer-consumer pipelines**.
* When you need **bounded queues** and **blocking semantics** without manual synchronization.
* When you want a simpler API compared to writing your own locks or `Monitor.Pulse` logic.


*/

/*
 * That behavior is exactly how `BlockingCollection` is designed to work when you give it a **bounded capacity** (in your case, `3`). Here's why your `Main` thread appears blocked:

---

### **How `BlockingCollection` Capacity Works**

* You created it like this:

  ```csharp
  private static BlockingCollection<string> collection =
      new BlockingCollection<string>(requestsQueue, 3);
  ```
* That means the collection can hold **a maximum of 3 items** at any time.
* When you call:

  ```csharp
  collection.Add(cusInput);
  ```

  **and the collection already contains 3 items**, `Add()` will **block the calling thread** (here, the `Main` thread) **until**:

  * A consumer removes at least one item (via `Take()` or `GetConsumingEnumerable()`), freeing up space.
  * Or the collection is marked complete and throws an exception if you keep adding.

---

### **Why It’s Blocking**

* `BlockingCollection` is meant for **producer-consumer** scenarios.
* The **producer** (your `Main` thread reading Console input) **should slow down** if the **consumer** (`MonitorQueue`) cannot process fast enough.
* This prevents memory from blowing up with unlimited input. It's a built-in **backpressure mechanism**.

---

### **What Happens Under the Hood**

* `BlockingCollection` wraps your `ConcurrentQueue` but **monitors item count internally**.
* When count == `3`:

  * Any further `Add()` calls wait (block) until another thread does a `Take()` (or a consumer finishes iterating).
* When the consumer takes or processes an item, capacity goes below `3`, and the waiting producer resumes.

---

### **Why You See This in Your Code**

* Your `MonitorQueue` uses:

  ```csharp
  foreach (var req in collection.GetConsumingEnumerable())
  {
      Thread processingThread = new Thread(() => ProcessInput(req));
      processingThread.Start();
      Thread.Sleep(1000);
  }
  ```
* This means:

  * It starts **processing items slowly** (1-second sleep per loop).
  * At first, the producer quickly fills the 3 slots.
  * Then it must **wait** each time until the consumer frees up a slot.

---

### **Key Points**

* This **blocking behavior is intentional**; it’s a key feature.
* If you remove the `3` (or set it to `-1`), it becomes **unbounded**, so the producer never blocks.
* If you want to avoid blocking but still limit size, you can use:

  ```csharp
  collection.TryAdd(item, timeout);
  ```

  which returns `false` if it can’t add in time.

---

### **Takeaway**

The blocking you're seeing is not an error—it’s **flow control**. It makes sure the producer doesn’t overwhelm the consumer.

 */