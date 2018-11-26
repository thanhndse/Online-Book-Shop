using System.Data.Entity;
using System.Data.Entity.Validation;

namespace BookShopWithAuthen.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
        void Dispose();
        void BeginTransaction();
        bool EndTransaction();
        void RollBack();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext dataContext;
        private DbContextTransaction transaction;
        private IDatabaseFactory databaseFactory;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        protected ApplicationDbContext DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }

        public void BeginTransaction()
        {
            transaction = DataContext.Database.BeginTransaction();
        }

        public bool EndTransaction()
        {
            try
            {
                DataContext.SaveChanges();
                transaction.Commit();
            }
            catch (DbEntityValidationException dbEx)
            {
                return false;
            }
            return true;
        }

        public void RollBack()
        {
            transaction.Rollback();
            Dispose();
        }

        public void Dispose()
        {
            transaction?.Dispose();
            DataContext?.Dispose();
        }
        public void Commit()
        {
            DataContext.SaveChanges();
        }
    }

    
}

