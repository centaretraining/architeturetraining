using System;
using System.Threading;

namespace Resiliency.Client
{
    internal class RequestProducer : IProducer<ConsumerInput>
    {
        private int _requests;
        private int _requestCount;
        private readonly int _waitMs;
        private readonly Uri _url;

        public RequestProducer(int waitMs, int requests, Uri url)
        {
            _waitMs = waitMs;
            _url = url;
            _requests = requests;
            _requestCount = 0;
        }

        public bool GetNext(out ConsumerInput item)
        {
            if (_requestCount == _requests)
            {
                item = null;
                return false;
            }

            _requestCount++;
            var url = _url.ToString().Contains("?")
                ? $"{_url}&requestNumber={_requestCount}"
                : $"{_url}?requestNumber={_requestCount}";
            item = new ConsumerInput()
            {
                Url = new Uri(url),
                RequestNumber = _requestCount
            };

            Thread.Sleep(_waitMs);

            return true;
        }
    }
}