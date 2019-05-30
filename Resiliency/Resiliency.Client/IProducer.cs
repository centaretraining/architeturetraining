namespace Resiliency.Client
{
    interface IProducer<T>
    {
        bool GetNext(out T item);
    }
}