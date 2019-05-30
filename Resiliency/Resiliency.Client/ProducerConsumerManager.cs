using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resiliency.Client
{
    internal class ProducerConsumerManager<TProducerOutput, TConsumerOutput>
    {
        private readonly IProducer<TProducerOutput> _producer;
        private readonly Func<IConsumer<TProducerOutput, TConsumerOutput>> _consumerFactory;
        private readonly int _producerPrimeMilliseconds;
        private ConcurrentQueue<TProducerOutput> _processingQueue;
        private ConcurrentQueue<TConsumerOutput> _completeQueue;
        public List<TConsumerOutput> CompletedItems { get; private set; }

        private bool _producerComplete;
        private CancellationTokenSource _cancellationSource;
        private Task _producerTask;
        private ICollection<Task> _consumerTasks;

        public EventHandler<StatusChangedEventArgs<TConsumerOutput>> StatusChanged;
        private Task _outputTask;
        private bool _isComplete;

        public ProducerConsumerManager(
            IProducer<TProducerOutput> producer,
            Func<IConsumer<TProducerOutput, TConsumerOutput>> consumerFactory,
            int producerPrimeMilliseconds = 0)
        {
            _producer = producer;
            _consumerFactory = consumerFactory;
            _producerPrimeMilliseconds = producerPrimeMilliseconds;
        }

        public void Start(int threadCount)
        {
            _cancellationSource = new CancellationTokenSource();
            _processingQueue = new ConcurrentQueue<TProducerOutput>();
            _completeQueue = new ConcurrentQueue<TConsumerOutput>();
            CompletedItems = new List<TConsumerOutput>();

            _producerTask = Task.Factory.StartNew(
                () => Produce(_cancellationSource.Token), TaskCreationOptions.LongRunning);

            _outputTask = Task.Factory.StartNew(
                () => Output(_cancellationSource.Token));

            // Let the producer fill up the queue for _producerPrimeMilliseconds ms
            Thread.Sleep(_producerPrimeMilliseconds);

            _consumerTasks = new List<Task>();
            for (int i = 0; i < threadCount; i++)
            {
                _consumerTasks.Add(
                    Task.Factory.StartNew(() => Consume(_cancellationSource.Token), TaskCreationOptions.LongRunning));
            }

            var consumersDone = Task.WhenAll(new[] {_producerTask}.Union(_consumerTasks));
            consumersDone.ContinueWith(t =>
            {
                // Producer and consumers should be done, this just cancels the output thread.
                _cancellationSource.Cancel();
            });
        }

        public void Stop()
        {
            _cancellationSource.Cancel();

            Task.WaitAll(
                new[] { _producerTask, _outputTask }
                    .Union(_consumerTasks)
                    .ToArray());

            ProcessCompleteQueue();
            _isComplete = true;
        }

        public bool IsComplete { get { return _isComplete; }}

        private void Produce(CancellationToken cancellationToken)
        {
            TProducerOutput item;
            while (_producer.GetNext(out item))
            {
                _processingQueue.Enqueue(item);

                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
            }

            _producerComplete = true;
        }

        private void Consume(CancellationToken cancellationToken)
        {
            var consumer = _consumerFactory();

            while (!cancellationToken.IsCancellationRequested)
            {
                TProducerOutput msg;
                if (_processingQueue.TryDequeue(out msg))
                {
                    var resp = consumer.Process(msg);
                    _completeQueue.Enqueue(resp);
                    CompletedItems.Add(resp);
                }
                else if (_producerComplete)
                {
                    // Done processing all items
                    return;
                }
                Thread.Yield();
            }
        }

        private void Output(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ProcessCompleteQueue();

                Thread.Sleep(100);
            }
            _isComplete = true;
        }

        private void ProcessCompleteQueue()
        {
            TConsumerOutput msg;
            var msgs = new List<TConsumerOutput>();
            while (_completeQueue.TryDequeue(out msg))
            {
                msgs.Add(msg);
            }

            if (StatusChanged != null)
            {
                StatusChanged(
                    this,
                    new StatusChangedEventArgs<TConsumerOutput>(
                        msgs.ToArray(), 
                        _processingQueue.Count));
            }
        }
    }
}