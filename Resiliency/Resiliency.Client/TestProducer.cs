using System.Threading;

namespace Resiliency.Client
{
    class TestProducer : IProducer<int>
    {
        private int _i = 0;

        public bool GetNext(out int item)
        {
            if (_i == 1000)
            {
                item = 0;
                return false;
            }

            // Take some time to produce the next item
            Thread.Sleep(100);

            item = _i;
            _i++;

            return true;
        }
    }
}