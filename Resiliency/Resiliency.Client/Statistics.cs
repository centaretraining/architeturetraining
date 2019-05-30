using System;

namespace Resiliency.Client
{
    public class Statistics
    {
        public Statistics()
        {
            MinRequestTime = TimeSpan.MaxValue;
        }

        public int TotalRequestCount { get; set; }

        public int SuccessfulRequestCount { get; set; }

        public int FailedRequestCount => TotalRequestCount - SuccessfulRequestCount;

        public double SuccessPercent => (double) SuccessfulRequestCount / (double) TotalRequestCount * (double) 100;

        public TimeSpan TotalRequestTime { get; set; }

        public TimeSpan MinRequestTime { get; set; }

        public TimeSpan MaxRequestTime { get; set; }

        public TimeSpan AverageRequestTime => TimeSpan.FromMilliseconds(TotalRequestTime.TotalMilliseconds / TotalRequestCount);
    }
}