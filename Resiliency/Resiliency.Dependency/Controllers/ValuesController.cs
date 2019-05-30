using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Resiliency.Dependency.Controllers
{
    [Route("")]
    public class DatesController : Controller
    {
        private static readonly Dictionary<int, Semaphore> ResourcePools;

        static DatesController()
        {
            ResourcePools = new Dictionary<int, Semaphore>();
        }

        [HttpGet]
        public IActionResult Get(int? failPercent, int? timeMs)
        {
            if (timeMs.HasValue)
            {
                Thread.Sleep(timeMs.Value);
            }

            if (failPercent.HasValue)
            {
                if (failPercent >= 100 || new Random().Next(0, 99) < failPercent)
                {
                    return StatusCode(500, "Service failure!");
                }
            }

            return Ok(GetRandomMenuItem());
        }

        [HttpGet("restrict")]
        public IActionResult Concurrent(int count, int timeMs, int waitMs)
        {
            if (!ResourcePools.ContainsKey(count))
            {
                lock (ResourcePools)
                {
                    if (!ResourcePools.ContainsKey(count))
                    {
                        ResourcePools.Add(count, new Semaphore(count, count));
                    }
                }
            }

            if (!ResourcePools[count].WaitOne(waitMs))
            {
                return StatusCode(503, "Service overwhelmed!");
            }

            Thread.Sleep(timeMs);
            ResourcePools[count].Release();

            return Ok(GetRandomMenuItem());
        }

        private string GetRandomMenuItem()
        {
            var items = new[]
            {
                "Bacon cheeseburger - $7",
                "Turkey burger - $6",
                "Grilled cheese - $4",
                "Hotdog - $3",
                "Soda - $1",
                "Milk - $1",
                "Orange Juice - $1",
                "Salad - $5",
                "Spaghetti and Meatballs - $7",
                "Chicken Noodle Soup - $2",
                "Pancakes - $4",
                "Tuna salad sandwich - $5"
            };
            var rand = new Random();
            var i = rand.Next(0, items.Length - 1);
            return items[i];
        }
    }
}
