namespace Resiliency.Client
{
    interface IConsumer<TIn, TOut>
    {
        TOut Process(TIn item);
    }
}