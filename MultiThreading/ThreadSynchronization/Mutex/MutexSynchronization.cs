namespace MultiThreading.ThreadSynchronization.Mutex;

using System.Threading;

public class MutexSynchronization
{
    // Mutex is very similar to Lock/Monitor. Another way to provide Synchronization.
    // But one thing that distinguishes the Mutex is, It not only can be used in the PROCESS,
    // but can also be used across PROCESSES.
    static void Main_old(string[] args)
    {
        string filePath =
            @"C:\\Users\\kbb91\\Desktop\\MultiThreading\\MultiThreading\\ThreadSynchronization\\Mutex\\counter.txt";

        using (var mutex = new Mutex(false, $"Global\\FileMutex"))
        {
            for (int i = 0; i < 10000; i++)
            {
                mutex.WaitOne();
                try
                {
                    int counter = ReadCounter(filePath);
                    counter++;
                    WriteCounter(filePath, counter);
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
        }

        Console.WriteLine("Process finished");
    }

    private static void WriteCounter(string filePath, int counter)
    {
        using (
            var stream = new FileStream(
                filePath,
                FileMode.OpenOrCreate,
                FileAccess.Write,
                FileShare.ReadWrite
            )
        )
        using (var writer = new StreamWriter(stream))
        {
            writer.Write(counter);
        }
    }

    private static int ReadCounter(string filePath)
    {
        using (
            var stream = new FileStream(
                filePath,
                FileMode.OpenOrCreate,
                FileAccess.Read,
                FileShare.ReadWrite
            )
        )
        using (var reader = new StreamReader(stream))
        {
            string content = reader.ReadToEnd();
            return string.IsNullOrEmpty(content) ? 0 : int.Parse(content);
        }
    }
}
