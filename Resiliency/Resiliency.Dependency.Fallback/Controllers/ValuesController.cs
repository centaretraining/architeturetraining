using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Resiliency.Dependency.Fallback.Controllers
{
    [Route("")]
    public class DatesController : Controller
    {
        [HttpGet]
        public IActionResult Get(int requestNumber, int? failPercent, int? timeMs)
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

            return Ok("Water - Free");
        }
    }
}
