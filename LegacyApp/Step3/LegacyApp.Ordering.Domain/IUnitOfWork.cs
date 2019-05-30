namespace LegacyApp.Ordering.Domain
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
