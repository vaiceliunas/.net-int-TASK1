/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    internal class Program
    {
        private static readonly AutoResetEvent ReadingEventHandler = new AutoResetEvent(true);
        private static readonly AutoResetEvent WritingEventHandler = new AutoResetEvent(false);

        private const int CollectionSize = 10;
        internal static List<int> SharedCollection = new List<int>();
        private static void Main()
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.");
            Console.WriteLine();

            Task.Factory.StartNew(AddElements);
            Task.Factory.StartNew(PrintElements);

            Console.ReadLine();
        }

        private static void PrintElements()
        {
            while(GetCollectionSize() < CollectionSize)
            {
                //waiting until writingEvent signals
                WritingEventHandler.WaitOne();

                lock(SharedCollection)
                {
                    var printMessage = "[" + string.Join(", ", SharedCollection.ToArray()) + "]";
                    Console.WriteLine(printMessage);
                }
                //signaling that reading(printing) is done
                ReadingEventHandler.Set();
            }
        }
        private static void AddElements()
        {
            for (var i = 0; i < CollectionSize; i++)
            {
                //waiting until readingEvent signals (for first iteration, its already signaled via constructor)
                ReadingEventHandler.WaitOne();

                lock (SharedCollection)
                {
                    if (!SharedCollection.Contains(i))
                        SharedCollection.Add(i);
                }
                //signaling that writing is done
                WritingEventHandler.Set();
            }
        }

        private static int GetCollectionSize()
        {
            lock (SharedCollection)
            {
                return SharedCollection.Count();
            }
        }
    }
}
