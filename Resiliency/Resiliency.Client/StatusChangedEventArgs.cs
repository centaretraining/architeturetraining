namespace Resiliency.Client
{
    class StatusChangedEventArgs<TConsumerOutput>
    {
        public StatusChangedEventArgs(
            TConsumerOutput[] statusChanges,
            int processingQueueLength)
        {
            StatusChanges = statusChanges;
            ProcessingQueueLength = processingQueueLength;
        }

        public TConsumerOutput[] StatusChanges { get; private set; }

        public int ProcessingQueueLength { get; private set; }
    }
}