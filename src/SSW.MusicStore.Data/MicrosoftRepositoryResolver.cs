using System;

using Microsoft.Extensions.DependencyInjection;

using SSW.MusicStore.Data.Interfaces;

namespace SSW.MusicStore.Data
{
    public class MicrosoftRepositoryResolver : IRepositoryResolver
    {
        private readonly IServiceProvider serviceProvider;

        public MicrosoftRepositoryResolver(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IRepository<TEntity> Resolve<TEntity>() where TEntity : class
        {
            return this.serviceProvider.GetService<IRepository<TEntity>>();
        }
    }
}
