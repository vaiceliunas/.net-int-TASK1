/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    internal class Program
    {
        private static int _integerState = 10;
        private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1);
        private static void Main()
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            CreateThreadsA();
            //CreateThreadsB();

            Console.ReadLine();
        }

        private static void CreateThreadsA()
        {
            //to get out of recursion
            if (_integerState == 0)
                return;

            _integerState--;
            var stateVariable = _integerState;
            var t = new Thread(() => PrintState(stateVariable));
            t.Start();
            t.Join();

            CreateThreadsA();
        }

        private static void CreateThreadsB()
        {
            if (_integerState == 0)
                return;

            _integerState--;
            SemaphoreSlim.Wait();
            ThreadPool.QueueUserWorkItem(new WaitCallback(PrintState), _integerState);
            SemaphoreSlim.Release();

            CreateThreadsB();
        }

        private static void PrintState (object state)
        {
            Console.WriteLine("integerState state value: " + state);
        }
    }
}
