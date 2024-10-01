using System;
using System.Threading;


class Program
{
    static void Main(string[] args)
    {
        Manager manager = new Manager();
        manager.StartWork();
        manager.WaitUntilWorkersFinished();
        Console.ReadLine();
    }


    public class Manager
    {
        private readonly object _lock = new object();
        private int _workersFinished = 0;

        public void StartWork()
        {
            Console.WriteLine("Начальник начал работу");
            for (int i = 0; i < 3; i++)
            {
                Thread worker = new Thread(Worker_DoWork);
                worker.Start();
            }
        }

        private void Worker_DoWork()
        {
            Console.WriteLine("Рабочий начал работу");
            
            Thread.Sleep(5000);
            Console.WriteLine("Рабочий закончил работу");
          
            lock (_lock)
            {
                _workersFinished++;
                if (_workersFinished == 3)
                {
                    Monitor.Pulse(_lock);
                }
            }
        }

        public void WaitUntilWorkersFinished()
        {
            lock (_lock)
            {
                while (_workersFinished < 3)
                {
                    Monitor.Wait(_lock);
                }
                Console.WriteLine("Все рабочие закончили работу");
            }
        }
    }
}