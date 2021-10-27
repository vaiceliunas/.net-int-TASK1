/*
 * 1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.
 * Each Task should iterate from 1 to 1000 and print into the console the following string:
 * “Task #0 – {iteration number}”.
 */
using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    class Program
    {
        private const int TaskAmount = 100;
        private const int MaxIterationsCount = 1000;


        private static void Main()
        {
            Console.WriteLine(".Net Mentoring Program. Multi threading V1.");
            Console.WriteLine("1.	Write a program, which creates an array of 100 Tasks, runs them and waits all of them are not finished.");
            Console.WriteLine("Each Task should iterate from 1 to 1000 and print into the console the following string:");
            Console.WriteLine("“Task #0 – {iteration number}”.");
            Console.WriteLine();

            HundredTasks();
            //HundredTasksParallel();

            Console.ReadLine();
        }

        private static void HundredTasks()
        {
            var tasksArray = new Task[TaskAmount];

            for (var i = 0; i < TaskAmount; i++)
            {
                //getting around for loop paradox;
                //https://dev.to/rickystam/c-the-for-loop-paradox-2l1j
                var taskNumber = i;
                tasksArray[taskNumber] = Task.Run(() => PrintIteration(taskNumber));
            }

            Task.WaitAll(tasksArray);
            Console.WriteLine("All tasks finished!");
        }

        private static void HundredTasksParallel()
        {
            var tasksArray = new Task[TaskAmount];

            Parallel.For(0, TaskAmount, taskNumber =>
            {
                tasksArray[taskNumber] = Task.Run(() => PrintIteration(taskNumber));
            });

            Task.WaitAll(tasksArray); 
            Console.WriteLine("All tasks finished!");
        }

        private static void PrintIteration(int i)
        {
            for (var j = 0; j < MaxIterationsCount; j++)
            {
                Output(i, j);
            }
        }

        private static void Output(int taskNumber, int iterationNumber)
        {
            Console.WriteLine($"Task #{taskNumber} – {iterationNumber}");
        }
    }
}
