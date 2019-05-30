using System;

namespace Resiliency.Client
{
    public class ConsumerOutput
    {
        public TimeSpan RequestTime { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}