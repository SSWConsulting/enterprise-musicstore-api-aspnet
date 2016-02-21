using Autofac;

using SSW.MusicStore.Data.Interfaces;

namespace SSW.MusicStore.DependencyResolution
{
    public class AutofacRepositoryResolver : IRepositoryResolver
    {
        private readonly ILifetimeScope lifetimeScope;

        public AutofacRepositoryResolver(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public IRepository<TEntity> Resolve<TEntity>() where TEntity : class
        {
            return this.lifetimeScope.Resolve<IRepository<TEntity>>();
        }
    }
}
