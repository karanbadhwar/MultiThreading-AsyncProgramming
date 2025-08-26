using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.Async_Await;

public class AwaitAndSynchronizationContext
{
    static void Main_old(string[] args)
    {
        // If we are running something on a different Thread other than the UI thread,
            // we can have the Synchronization context problem like we had in the WinFORM
            // await keywords makes everything run in continuation and everything in,
                // continuation is running on the same context.

        // Example Form Example.

    }
}
