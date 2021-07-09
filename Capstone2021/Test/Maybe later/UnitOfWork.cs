using Capstone2021.DTO;
using Capstone2021.Interfaces;
using System;

namespace Capstone2021.Utils
{
    public class UnitOfWork : IUnitOfWork
    {
        private DbEntities context;
        private IGenericRepository<Recruiter> recruiterRepository;
        public UnitOfWork(DbEntities context)
        {
            this.context = context;
        }

        public IGenericRepository<Recruiter> RecruiterRepository => throw new NotImplementedException();


        public int Save()
        {
            return context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
