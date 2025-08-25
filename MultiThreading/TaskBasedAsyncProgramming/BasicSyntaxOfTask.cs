using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiThreading.TaskBasedAsyncProgramming
{
    public class BasicSyntaxOfTask
    {
        static void Main_old(string[] args)
        {
            // using thread
            //Thread thread = new Thread(Work);
            //thread.Start();
            /*
             * Thread ->
             * When you start a Thread manually, it creates a foreground thread by default.
            Foreground threads keep the process alive until they finish.
            Even if Main ends, the process doesn’t terminate until all foreground threads are done.
            So the console doesn’t disappear immediately because the process is still running until your thread finishes printing.
             * */

            // using Task
            //Task task = new Task(Work);
            //task.Start();
            /*
             * Task uses the ThreadPool, which runs tasks on background threads by default.
                Background threads do not keep the process alive.
                If Main ends, the process terminates immediately, and any running background threads are killed.
                That’s why without Console.ReadLine() or task.Wait(), your program exits before you see the output.
             */

            // Other way to run a task and also returns a Task Object
            Task task = Task.Run(Work);
            // gives a lot of methods!!!

            Console.ReadLine();
        }

        public static void Work()
        {
            Console.WriteLine("I love programming!");
        }
    }
}
