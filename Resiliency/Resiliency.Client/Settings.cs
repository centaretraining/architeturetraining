using System;

namespace Resiliency.Client
{
    internal class Settings
    {
        public int WaitMs { get; set; }
        public int Requests { get; set; }
        public Uri Url { get; set; }
    }
}