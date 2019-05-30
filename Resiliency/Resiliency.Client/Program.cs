using System;
using System.Threading;

namespace Resiliency.Client
{
    class Program
    {
        private const string BaseUrl = "http://localhost:5000/";
        private const int ConsumerThreads = 20;
        private static Statistics _statistics = new Statistics();

        static void Main(string[] args)
        {
            var settings = GetSettings();
            while (settings != null)
            {
                Run(settings);

                Console.WriteLine();
                settings = GetSettings();
            }
        }

        private static void Run(Settings settings)
        {
            _statistics = new Statistics();

            Console.WriteLine(
                $"Sending {(settings.Requests <= 0 ? "" : settings.Requests + " ")}requests every {settings.WaitMs} milliseconds to {settings.Url}...");

            var manager = new ProducerConsumerManager<ConsumerInput, ConsumerOutput>(
                //new TestProducer(),
                new RequestProducer(settings.WaitMs, settings.Requests, settings.Url),
                //() => new TestConsumer()
                () => new RequestConsumer()
            );

            manager.StatusChanged += ManagerItemsProcessedHandler;
            manager.Start(ConsumerThreads);

            while (!manager.IsComplete && !Console.KeyAvailable)
            {
                Thread.Sleep(100);
            }

            if (Console.KeyAvailable)
            {
                Console.ReadKey();
            }

            manager.Stop();

            Console.WriteLine("Hit any key to stop...");
            Console.ReadKey();

            Console.WriteLine();
            Console.WriteLine("Results:");
            Console.WriteLine($"Requests: {_statistics.TotalRequestCount}");
            Console.WriteLine(
                value: $"Succeeded: {_statistics.SuccessfulRequestCount} ({_statistics.SuccessPercent:G5}%)");
            Console.WriteLine($"Failed: {_statistics.FailedRequestCount}");
            Console.WriteLine($"Time Avg ms: {_statistics.AverageRequestTime.TotalMilliseconds:G}");
            Console.WriteLine($"Time Min ms: {_statistics.MinRequestTime.TotalMilliseconds:G}");
            Console.WriteLine($"Time Max ms: {_statistics.MaxRequestTime.TotalMilliseconds:G}");
        }

        private static Settings GetSettings()
        {
            // Demo service
            Console.WriteLine("Service Demos");
            Console.WriteLine(" 1) Successful calls");
            Console.WriteLine(" 2) 25% of calls fail");
            Console.WriteLine(" 3) Delay 2 seconds");
            Console.WriteLine(" 4) Delay 3 seconds, max 3 concurrent");
            Console.WriteLine("  4a) Burst of 10 messages for demo #4");

            // Retry
            Console.WriteLine("Retry Demos");
            Console.WriteLine(" 5) Retry twice on failures");
            Console.WriteLine(" 6) Max 3 concurrent, retry twice");
            Console.WriteLine("  6a) Burst of 5 calls for 5a");

            // Failover
            Console.WriteLine("Fall Back Demos");
            Console.WriteLine(" 7) Fall back to secondary service");

            // Circuit Breaker
            Console.WriteLine("Circuit Breaker Demos");
            Console.WriteLine(" 8) Max 3 concurrent, circuit break and fall back");
            Console.WriteLine("  8a) Burst of 10 calls for #7");

            // Bulkhead isolation
            Console.WriteLine(" 9) Max 3 concurrent, bulkhead isolate to 3");
            Console.WriteLine("  9a) Burst of 10 calls for #8");

            Console.WriteLine(" 99) Manual");

            Console.WriteLine(" q) Quit");

            var opt = ReadString("Option");

            Settings settings = null;
            switch (opt)
            {
                case "1":
                    settings = new Settings()
                    {
                        WaitMs = 1000,
                        Requests = 10,
                        Url = new Uri(BaseUrl + "demo1")
                    };
                    break;
                case "2":
                    settings = new Settings()
                    {
                        WaitMs = 1000,
                        Requests = 10,
                        Url = new Uri(BaseUrl + "demo2")
                    };
                    break;
                case "3":
                    settings = new Settings()
                    {
                        WaitMs = 1000,
                        Requests = 10,
                        Url = new Uri(BaseUrl + "demo3")
                    };
                    break;
                case "4":
                    settings = new Settings()
                    {
                        WaitMs = 1000,
                        Requests = -1,
                        Url = new Uri(BaseUrl + "demo4")
                    };
                    break;
                case "4a":
                    settings = new Settings()
                    {
                        WaitMs = 100,
                        Requests = 10,
                        Url = new Uri(BaseUrl + "demo4")
                    };
                    break;
                case "5":
                    settings = new Settings()
                    {
                        WaitMs = 1000,
                        Requests = 10,
                        Url = new Uri(BaseUrl + "demo5")
                    };
                    break;
                case "6":
                    settings = new Settings()
                    {
                        WaitMs = 1000,
                        Requests = -1,
                        Url = new Uri(BaseUrl + "demo5a")
                    };
                    break;
                case "6a":
                    settings = new Settings()
                    {
                        WaitMs = 100,
                        Requests = 5,
                        Url = new Uri(BaseUrl + "demo5a")
                    };
                    break;
                case "7":
                    settings = new Settings()
                    {
                        WaitMs = 1000,
                        Requests = 10,
                        Url = new Uri(BaseUrl + "demo6")
                    };
                    break;
                case "8":
                    settings = new Settings()
                    {
                        WaitMs = 1000,
                        Requests = -1,
                        Url = new Uri(BaseUrl + "demo7")
                    };
                    break;
                case "8a":
                    settings = new Settings()
                    {
                        WaitMs = 100,
                        Requests = 10,
                        Url = new Uri(BaseUrl + "demo7")
                    };
                    break;
                case "9":
                    settings = new Settings()
                    {
                        WaitMs = 1000,
                        Requests = -1,
                        Url = new Uri(BaseUrl + "demo8")
                    };
                    break;
                case "9a":
                    settings = new Settings()
                    {
                        WaitMs = 100,
                        Requests = 10,
                        Url = new Uri(BaseUrl + "demo8")
                    };
                    break;
                case "99":
                    settings = new Settings();
                    settings.WaitMs = ReadInt("Time (ms)");
                    settings.Requests = ReadInt("Number of requests");
                    settings.Url = ReadUri("Request Uri", BaseUrl);
                    break;
                case "q":
                    settings = null;
                    break;
            }

            return settings;
        }

        private static void ManagerItemsProcessedHandler(object sender, StatusChangedEventArgs<ConsumerOutput> e)
        {
            foreach (var item in e.StatusChanges)
            {
                _statistics.TotalRequestCount++;
                _statistics.TotalRequestTime += item.RequestTime;
                if (_statistics.MinRequestTime > item.RequestTime)
                {
                    _statistics.MinRequestTime = item.RequestTime;
                }
                if (_statistics.MaxRequestTime < item.RequestTime)
                {
                    _statistics.MaxRequestTime = item.RequestTime;
                }

                if (item.Success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    _statistics.SuccessfulRequestCount++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.WriteLine(item.Message);
                Console.ResetColor();
            }
        }

        private static string ReadString(string prompt)
        {
            var value = "";

            while (string.IsNullOrWhiteSpace(value))
            {
                Console.Write(prompt + ": ");
                value = Console.ReadLine();
            }

            return value;
        }

        private static int ReadInt(string prompt)
        {
            int value = 0;
            var parsed = false;

            while (!parsed)
            {
                Console.Write(prompt + ": ");
                var input = Console.ReadLine();
                parsed = int.TryParse(input, out value);
            }

            return value;
        }

        private static Uri ReadUri(string prompt, string baseUrl)
        {
            Uri value = null;
            var parsed = false;

            while (!parsed)
            {
                Console.Write(prompt + ": " + baseUrl);
                var input = Console.ReadLine();
                try
                {
                    value = new Uri(baseUrl + input);
                    parsed = true;
                }
                catch
                {
                    // ignored
                }
            }

            return value;
        }
    }
}
