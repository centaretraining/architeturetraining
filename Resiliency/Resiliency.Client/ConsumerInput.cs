using System;

namespace Resiliency.Client
{
    public class ConsumerInput
    {
        public Uri Url { get; set; }

        public int RequestNumber { get; set; }
    }
}