using System;
using System.Threading;
using System.Collections.Concurrent;
using System.Linq;

namespace ThreadPools
{
    class Program
    {
        public Program(int threadsCount, int numberOfScores)
        {
            _ThreadsCount = threadsCount;
            _ThreadsCountdown = new CountdownEvent(threadsCount);
            _NumberOfScores = numberOfScores;
        }

        private ConcurrentQueue<Int32> _Scores = new ConcurrentQueue<int>();
        private CountdownEvent _ThreadsCountdown;
        private int _ThreadsCount;
        private int _NumberOfScores;

        static void Main(string[] args)
        {
            Program program = new Program(10, 1000000);
            program.Run();
        }

        public void Run()
        {
            Random randomGenerator = new Random();
            for(int i = 0; i < _ThreadsCount; i++)
            {
                ThreadPool.QueueUserWorkItem(x =>
                {
                    Console.WriteLine($"Thread n° {Thread.CurrentThread.ManagedThreadId} has started");
                    while(_Scores.Count < _NumberOfScores)
                    {
                        _Scores.Enqueue(randomGenerator.Next(1, 100));
                    }
                    _ThreadsCountdown.Signal();
                });
            }
            _ThreadsCountdown.Wait(1000);
            Console.WriteLine("All threads finished");
            Console.WriteLine("Number of items in queue : " + _Scores.ToList().Count());
           // _Scores.ToList().ForEach(x => Console.Write(x + " "));
        }

        
    }
}
