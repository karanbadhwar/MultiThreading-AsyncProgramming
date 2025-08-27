using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.ConcurrentCollections;

public class ConcurrentStack
{

    static void Main_old(string[] args)
    {
        ConcurrentStack<int> stack = new ConcurrentStack<int>();

        stack.Push(1);
        stack.Push(2);
        stack.Push(3);


        stack.TryPop(out var result);
        

        Console.WriteLine(result);
    }
}
