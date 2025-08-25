using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming;

public class MultiThreading_AsyncProgramming
{
    static void Main_old(string[] args)
    {
        // M.T / A.P -> both are very similar.
        // Example in M.T, in M.T we divide and conquer, means we utilize  multiple threads
            // to divide and conquer a task and where multiple tasks can be run Concurrently.
            // Use case -> CPU Bound cases.

        // Example in A.P, in A.P we offload the long running tasks and make them run in a
            // diff thread, it is also M.T but emphasis over here was to move the big task to side
            // to vacant the Main Thread.
            // Use case -> I/O Bound.
    }
}
