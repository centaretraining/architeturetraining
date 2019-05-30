using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Resiliency.Client
{
    internal class Demo
    {
        private Task[] _tasks;
        private readonly CancellationTokenSource _cancellationSource = new CancellationTokenSource();
        private CancellationToken _cancellationToken;
        private readonly ConcurrentBag<Stat> _stats = new ConcurrentBag<Stat>();

        public IEnumerable<Stat> Stats => _stats;

        public void Start(Uri url, int threads, int waitMs, int requests)
        {
            _tasks = new Task[threads];
            _cancellationToken = _cancellationSource.Token;

            for (int i = 0; i < threads; i++)
            {
                var threadNo = i;
                _tasks[i] = Task.Run(
                    () => Loop(threadNo, url, waitMs, requests),
                    _cancellationSource.Token);
            }
        }

        public void Stop()
        {
            Console.WriteLine("Stopping...");
            _cancellationSource.Cancel();
            Task.WaitAll(_tasks);
        }

        private async Task Loop(int threadNumber, Uri url, int waitMs, int requests)
        {
            var httpClient = new HttpClient();

            if (_cancellationToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                // Warmup request
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                await httpClient.SendAsync(request);
            }
            catch
            {
                // ignore
            }

            var itr = 0;
            while (itr != requests)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await SendRequest(threadNumber, url, httpClient);

                if (_cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                Thread.Sleep(waitMs);

                if (_cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                itr++;
            }

            Console.WriteLine($"Thread #{threadNumber} done");
        }

        private async Task SendRequest(int threadNumber, Uri url, HttpClient httpClient)
        {
            Console.WriteLine($"Sending request on thread #{threadNumber}...");
            var request = new HttpRequestMessage(HttpMethod.Get, url);

            HttpResponseMessage response;
            Stat stat = new Stat();
            _stats.Add(stat);
            var timer = new Stopwatch();
            timer.Start();
            try
            {
                response = await httpClient.SendAsync(request);
            }
            catch (Exception e)
            {
                timer.Stop();
                stat.Success = false;
                stat.Milliseconds = timer.ElapsedMilliseconds;

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error calling service: {e.GetType()} - {e.Message}");
                Console.ResetColor();
                return;
            }

            timer.Stop();
            stat.Milliseconds = timer.ElapsedMilliseconds;

            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                stat.Success = true;
            }
            else
            { 
                Console.ForegroundColor = ConsoleColor.Red;
                stat.Success = false;
            }

            Console.WriteLine($"Response received: {(int)response.StatusCode} {response.ReasonPhrase} - {content}");
            Console.ResetColor();
        }
    }
}