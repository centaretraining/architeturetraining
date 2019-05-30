using System;
using System.Diagnostics;
using System.Threading;

namespace Resiliency.Client
{
    internal class TestConsumer : IConsumer<int, long>
    {
        private Stopwatch _stopWatch = new Stopwatch();

        public long Process(int input)
        {
            var r = new Random();
            var ms = r.Next(500, 3000);

            _stopWatch.Restart();

            Thread.Sleep(ms);

            _stopWatch.Stop();

            return _stopWatch.ElapsedMilliseconds;
        }
    }
}