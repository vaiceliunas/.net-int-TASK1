/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task6.Continuation
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
            Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
            Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
            Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
            Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
            Console.WriteLine("Demonstrate the work of the each case with console utility.");
            Console.WriteLine();

            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            Task.Factory.StartNew(() => 
            {
                Thread.Sleep(2500);
                //tokenSource.Cancel();
            });

            var analyzeWeather = Task.Run(() =>
            {
                Console.WriteLine("(A) thread ID" + Thread.CurrentThread.ManagedThreadId);
                //throw new Exception("exc");
                var results = new WeatherAnalyzerResults
                {
                    StartTime = DateTime.Now
                };

                var analyzerCycle = 0;
                while(analyzerCycle < 10)
                {
                    if (token.IsCancellationRequested)
                        token.ThrowIfCancellationRequested();

                    Thread.Sleep(500);
                    analyzerCycle++;
                    Console.WriteLine("Weather analyzer cycle " + analyzerCycle);
                }                
                results.EndTime = DateTime.Now;
                results.WeatherIn12Hours = "Sunny";
                results.WeatherIn24Hours = "Rainy";
                return results;
            }, token);

            var continuationA = analyzeWeather.ContinueWith((x) =>
            {
                Console.WriteLine("(A)Continues regardless of the result");
                if(x.Status == TaskStatus.RanToCompletion)
                    Console.WriteLine("Analyzer results: In 12 hours it's gonna be " + x.Result.WeatherIn12Hours + ", in 24 hours it's gonna be " + x.Result.WeatherIn24Hours);
            }, CancellationToken.None);

            var continuationB = analyzeWeather.ContinueWith((x) =>
            {
                Console.WriteLine("(B)Continues when parent task finished without success");

            }, TaskContinuationOptions.NotOnRanToCompletion);

            var continuationC = analyzeWeather.ContinueWith((x) =>
            {

                Console.WriteLine("(C)Continues when parent task finished without success");
                Console.WriteLine("(C) thread ID" + Thread.CurrentThread.ManagedThreadId);

                //now on same thread, but executes all the time. I gave up
            }, TaskContinuationOptions.OnlyOnFaulted).ConfigureAwait(false);

            var continuationD = analyzeWeather.ContinueWith((x) =>
            {
                analyzeWeather.ContinueWith((y) =>
                {
                    Console.WriteLine("(D)Continues when parent task is cancelled");
                    Console.WriteLine("(D)Is current thread in thread pool?" + Thread.CurrentThread.IsThreadPoolThread);
                }, TaskContinuationOptions.LongRunning);

            }, TaskContinuationOptions.OnlyOnCanceled);

            Console.ReadLine();
        }
    }

    internal class WeatherAnalyzerResults
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string WeatherIn12Hours { get; set; }
        public string WeatherIn24Hours { get; set; }
    }
}
