using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using SSW.MusicStore.Data.Interfaces;

namespace SSW.MusicStore.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextScope dbContextScope;

        private readonly IRepositoryLocator repositoryLocator;

        public UnitOfWork(IDbContextScope dbContextScope, IRepositoryLocator repositoryLocator)
        {
            this.dbContextScope = dbContextScope;
            this.repositoryLocator = repositoryLocator;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return this.repositoryLocator.GetRepository<TEntity>();
        }

        public void SaveChanges()
        {
            this.dbContextScope.SaveChanges();
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await this.dbContextScope.SaveChangesAsync(cancellationToken);
        }

        private bool disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.dbContextScope.Dispose();
            }

            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }
    }
}
