using System;
using System.Diagnostics;
using System.Net.Http;

namespace Resiliency.Client
{
    internal class RequestConsumer : IConsumer<ConsumerInput, ConsumerOutput>
    {
        private static readonly HttpClient HttpClient;

        static RequestConsumer()
        {
            HttpClient = new HttpClient();
        }

        public ConsumerOutput Process(ConsumerInput item)
        {
            Console.WriteLine($"Sending request #{item.RequestNumber}...");
            var request = new HttpRequestMessage(HttpMethod.Get, item.Url);

            HttpResponseMessage response;
            var result = new ConsumerOutput();
            var timer = new Stopwatch();
            timer.Start();
            try
            {
                response = HttpClient.SendAsync(request).Result;
            }
            catch (Exception e)
            {
                timer.Stop();
                result.Success = false;
                result.RequestTime = timer.Elapsed;

                result.Message = $"Error calling service: {e.GetType()} - {e.Message}";
                return result;
            }

            timer.Stop();
            result.RequestTime = timer.Elapsed;

            var content = response.Content.ReadAsStringAsync().Result;
            result.Success = response.IsSuccessStatusCode;

            result.Message = $"Request #{item.RequestNumber} response received: {(int)response.StatusCode} {response.ReasonPhrase} - {content}";

            return result;
        }
    }
}