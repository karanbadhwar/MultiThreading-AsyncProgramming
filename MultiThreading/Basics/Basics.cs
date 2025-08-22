namespace MultiThreading.Basics;

public class Basics
{
    static void main_old(string[] args)
    {
        /*
         * 🔹 What does the CPU actually run?

        👉 The CPU always runs threads, not processes directly.
        Here’s why:
        A process is just a container (it has memory, files, resources, etc.).
        The actual work is done by threads inside the process.
        At any given instant, a CPU core executes one thread’s instructions (in a time slice, unless hyper-threading is in play).
        The OS scheduler decides which thread from which process gets CPU time.

        🔹 Example
        Suppose you run Chrome:
        Chrome (process) starts.
        Inside Chrome, there might be multiple threads:
        One thread for the UI,
        One for JavaScript execution,
        One for rendering,
        One for networking.
        The CPU doesn’t “see” Chrome the process — it only executes one of Chrome’s threads when the OS schedules it.

        ✅ So:
        CPU runs instructions → instructions belong to threads → threads belong to processes.
        That means CPU runs threads, and processes are just the containers.
        */

    //      🔹 When an application starts
            //    The Operating System(OS) creates a process for it.
            //    By default, that process starts with one thread:
            //the main thread(a.k.a.primary thread).
            //That thread is where your program’s main() (in C/C++/Java/C#) or entry point runs.
            //So even if you don’t write any threading code, your app already has at least one thread — created automatically by the OS.


    }
}
