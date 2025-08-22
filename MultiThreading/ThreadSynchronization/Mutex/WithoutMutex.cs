using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ThreadSynchronization.Mutex;

internal class WithoutMutex
{
    static void Main_old(string[] args)
    {
        string filePath = "C:\\Users\\kbb91\\Desktop\\MultiThreading\\MultiThreading\\ThreadSynchronization\\Mutex\\counter.txt";

        for (int i = 0; i < 10000; i++)
        {

            int counter = ReadCounter(filePath);
            counter++;
            WriteCounter(filePath, counter);
        }

        Console.WriteLine("Process finished");
    }

    private static void WriteCounter(string filePath, int counter)
    {
        using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
        using (var writer = new StreamWriter(stream))
        {
            writer.Write(counter);
        }
    }

    private static int ReadCounter(string filePath)
    {
        using (var stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(stream))
        {
            string content = reader.ReadToEnd();
            return string.IsNullOrEmpty(content) ? 0 : int.Parse(content);
        }
    }
}
