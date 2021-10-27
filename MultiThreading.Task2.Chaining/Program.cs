/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        private const int ArrSize = 10;
        private const int RandomValueMin = 1;
        private const int RandomValueMax = 1000;

        private static void Main()
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            // task1
            var firstTask = Task.Run(() =>
            {
                var intArray = new int[ArrSize];
                var random = new Random();

                for (var i = 0; i < ArrSize; i++)
                {
                    intArray[i] = random.Next(RandomValueMin, RandomValueMax);
                    Console.Write(intArray[i] + " ");
                }

                Console.WriteLine("task 1 is done");
                return intArray;
            });

            // task 2
            var secondTask = firstTask.ContinueWith((x) =>
            {
                var randomNumber = new Random().Next(RandomValueMin, RandomValueMax);
                Console.WriteLine("Random number - " + randomNumber);
                for (var i = 0; i < x.Result.Length; i++)
                {
                    x.Result[i] *= randomNumber;
                    Console.Write(x.Result[i] + " ");
                }
                Console.Write("\n");
                Console.WriteLine("task 2 is done");
                return x.Result;
            });

            // task 3
            var thirdTask = secondTask.ContinueWith(x =>
            {
                var list = Enumerable.OfType<int>(x.Result).ToList();
                list.Sort();

                foreach (var t in list)
                {
                    Console.Write(t + " ");
                }
                Console.Write("\n");
                Console.WriteLine("task 3 is done");
                return list.ToArray();
            });

            // task 4

            var fourthTask = thirdTask.ContinueWith(x =>
            {
                Console.WriteLine("task 4 is done");
                return x.Result.Average();
            });

            Console.WriteLine("Final result is: " + fourthTask.Result);

            Console.ReadLine();
        }
    }
}
