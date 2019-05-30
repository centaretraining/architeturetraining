using LegacyApp.Ordering.Domain;

namespace LegacyApp.Ordering.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LegacyAppDbContext _dbContext;

        public UnitOfWork(LegacyAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}