using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Polly;
using Polly.Bulkhead;

namespace Resiliency.Api.Controllers
{
    [Route("")]
    public class ValuesController : Controller
    {
        private const string DependencyUrl = "http://localhost:6000/";
        private const string FallbackUrl = "http://localhost:7000/";
        private const string CacheUrl = "http://localhost:8000/";
        private static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// Demo 1: Standard call to service, no errors or delays
        /// </summary>
        [HttpGet("demo1")]
        public async Task<IActionResult> Demo1(int requestNumber)
        {
            Console.WriteLine($"Request #{requestNumber} received");
            var result = await Execute(DependencyUrl, requestNumber);
            Console.WriteLine($"Request #{requestNumber} complete");
            return result;
        }

        /// <summary>
        /// Demo 2: 25% chance of failure
        /// </summary>
        [HttpGet("demo2")]
        public async Task<IActionResult> Demo2(int requestNumber)
        {
            Console.WriteLine($"Request #{requestNumber} received");
            var result = await Execute($"{DependencyUrl}?failPercent=25", requestNumber);
            Console.WriteLine($"Request #{requestNumber} complete");
            return result;
        }

        /// <summary>
        /// Demo 3: Call takes 2 seconds
        /// </summary>
        [HttpGet("demo3")]
        public async Task<IActionResult> Demo3(int requestNumber)
        {
            Console.WriteLine($"Request #{requestNumber} received");
            var result = await Execute($"{DependencyUrl}?timeMs=2000", requestNumber);
            Console.WriteLine($"Request #{requestNumber} complete");
            return result;
        }

        /// <summary>
        /// Demo 4: Call takes 3 seconds, can only handle 3 at a time, and waits 2 seconds for resources
        /// </summary>
        [HttpGet("demo4")]
        public async Task<IActionResult> Demo4(int requestNumber)
        {
            Console.WriteLine($"Request #{requestNumber} received");
            var result = await Execute($"{DependencyUrl}restrict?count=3&timeMs=3000&waitMs=100", requestNumber);
            Console.WriteLine($"Request #{requestNumber} complete");
            return result;
        }

        /// <summary>
        /// Demo 5: 25% chance of failure, 2 retries, call takes 500ms
        /// </summary>
        [HttpGet("demo5")]
        public async Task<IActionResult> Demo5(int requestNumber)
        {
            Console.WriteLine($"Request #{requestNumber} received");

            // Create a retry policy
            var policy = Policy.Handle<Exception>().RetryAsync(
                2,
                (ex, r) => { Console.WriteLine($"Retry #{r} for request #{requestNumber}"); });

            // Execute the call using the policy
            IActionResult result = null;
            await policy.ExecuteAsync(async () =>
            {
                result = await Execute($"{DependencyUrl}?failPercent=25&timeMs=500", requestNumber, true);
            });

            Console.WriteLine($"Request #{requestNumber} complete");
            return result;
        }

        /// <summary>
        /// Demo 5a: Falls back to backup service, all calls take 3 seconds with a max of 3 concurrent
        /// </summary>
        [HttpGet("demo5a")]
        public async Task<IActionResult> Demo5a(int requestNumber)
        {
            Console.WriteLine($"Request #{requestNumber} received");

            // Create a retry policy
            var policy = Policy.Handle<Exception>().RetryAsync(
                2,
                (ex, r) => { Console.WriteLine($"Retry #{r} for request #{requestNumber}"); });

            // Execute the call using the policy
            IActionResult result = null;
            await policy.ExecuteAsync(async () =>
            {
                result = await Execute($"{DependencyUrl}restrict?count=3&timeMs=3000&waitMs=100", requestNumber, true);
            });

            Console.WriteLine($"Request #{requestNumber} complete");
            return result;
        }

        /// <summary>
        /// Demo 6: 25% chance of failure, falls back to backup service, all calls take 500ms
        /// </summary>
        [HttpGet("demo6")]
        public async Task<IActionResult> Demo6(int requestNumber)
        {
            Console.WriteLine($"Request #{requestNumber} received");

            IActionResult result = null;

            // Create a fallback policy
            var policy = Policy.Handle<Exception>().FallbackAsync(
                async (cancellation) =>
                {
                    Console.WriteLine($"Request #{requestNumber} using fallback service");
                    result = await Execute(FallbackUrl, requestNumber, true);
                });

            // Execute the call using the policy
            await policy.ExecuteAsync(async () =>
            {
                result = await Execute($"{DependencyUrl}?failPercent=25", requestNumber, true);
            });

            Console.WriteLine($"Request #{requestNumber} complete");
            return result;
        }

        /// <summary>
        /// Demo 7: Max 3 concurrent calls, circuit breaker after 3 errors, falls back to backup service, all calls take 500ms
        /// </summary>
        [HttpGet("demo7")]
        public async Task<IActionResult> Demo7(int requestNumber)
        {
            Console.WriteLine($"Request #{requestNumber} received");

            IActionResult result = null;

            // Create a circuit breaker + fallback policy
            var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(
                2, TimeSpan.FromSeconds(5),
                (ex, ts, ctx) => { Console.WriteLine($"Request #{requestNumber} caused circuit to trip!"); },
                (ctx) => { Console.WriteLine("Circuit has been restored!"); });
            var fallbackPolicy = Policy.Handle<Exception>().FallbackAsync(
                async (cancellation) =>
                {
                    Console.WriteLine($"Request #{requestNumber} using fallback service");
                    result = await Execute(FallbackUrl, requestNumber, true);
                });

            // Execute the call using both policies
            await Policy.WrapAsync(circuitBreakerPolicy, fallbackPolicy)
                .ExecuteAsync(async () =>
                {
                    result = await Execute($"{DependencyUrl}restrict?count=3&timeMs=3000&waitMs=100", requestNumber, true);
                });

            Console.WriteLine($"Request #{requestNumber} complete");
            return result;
        }

        // Create a shared bulkhead isolation policy for demo 8
        private static readonly AsyncBulkheadPolicy IsolationPolicy = Policy.BulkheadAsync(3, 10);

        /// <summary>
        /// Demo 8: Max 3 concurrent calls, calls take 2 seconds, bulkhead isolate to 3 with a 10 deep queue
        /// </summary>
        [HttpGet("demo8")]
        public async Task<IActionResult> Demo8(int requestNumber)
        {
            Console.WriteLine($"Request #{requestNumber} received");

            IActionResult result = null;

            // Create fallback policy
            var fallbackPolicy = Policy.Handle<Exception>().FallbackAsync(
                async (cancellation) =>
                {
                    Console.WriteLine($"Request #{requestNumber} using fallback service");
                    result = await Execute(FallbackUrl, requestNumber, true);
                });

            // Execute the call using both policies
            await Policy.WrapAsync(IsolationPolicy, fallbackPolicy)
                .ExecuteAsync(async () =>
                {
                    result = await Execute($"{DependencyUrl}restrict?count=3&timeMs=2000&waitMs=100", requestNumber, true);
                });

            Console.WriteLine($"Request #{requestNumber} complete");
            return result;
        }

        private async Task<IActionResult> Execute(string url, int requestNumber, bool throwOnError = false)
        {
            Console.WriteLine($"Request #{requestNumber} sending back end request to {url}...");

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage response;
            try
            {
                response = await HttpClient.SendAsync(request);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Request #{requestNumber} back end service call failed: {e.GetType()} - {e.Message}");
                Console.ResetColor();

                var message = $"Request #{requestNumber} back end service call failed: {e.GetType()} - {e.Message}";
                if (throwOnError)
                {
                    throw new Exception(message);
                }
                return new ObjectResult(message)
                {
                    StatusCode = 500
                };
            }

            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            Console.WriteLine($"Request #{requestNumber} back end response received: {(int)response.StatusCode} {response.ReasonPhrase} - {content}");
            Console.ResetColor();

            if (!response.IsSuccessStatusCode)
            {
                var message =
                    $"Request #{requestNumber} back end service call failed: {(int) response.StatusCode} {response.ReasonPhrase} - {content}";
                if (throwOnError)
                {
                    throw new Exception(message);
                }
                return new ObjectResult(message)
                {
                    StatusCode = 500
                };
            }

            return new OkObjectResult($"Request #{requestNumber} back end call succeeded: {content}");
        }
    }
}
