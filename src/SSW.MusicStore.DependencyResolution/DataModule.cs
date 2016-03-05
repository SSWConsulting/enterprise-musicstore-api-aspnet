using Autofac;

using SSW.MusicStore.Data;
using SSW.MusicStore.Data.Entities;
using SSW.MusicStore.Data.Initializers;
using SSW.MusicStore.Data.Interfaces;

using Module = Autofac.Module;

namespace SSW.MusicStore.DependencyResolution
{
    public class DataModule : Module
    {
        private readonly string connectionString;

        private readonly IDatabaseInitializer databaseInitializer;

        public DataModule(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public DataModule(string connectionString, IDatabaseInitializer databaseInitializer)
        {
            this.connectionString = connectionString;
            this.databaseInitializer = databaseInitializer;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<DbContextFactory>()
                .As<IDbContextFactory>()
                .WithParameter("connectionString", this.connectionString);

            if (this.databaseInitializer == null)
            {
                builder.RegisterType<MigrateToLatestVersion>().As<IDatabaseInitializer>();
            }
            else
            {
                builder.RegisterInstance(this.databaseInitializer).As<IDatabaseInitializer>();
            }
            
            builder.RegisterType<DbContextScope>().As<IDbContextScope>();
            builder.RegisterType<DbContextReadOnlyScope>().As<IDbContextReadOnlyScope>();
            builder.RegisterType<AutofacRepositoryResolver>().As<IRepositoryResolver>();
            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>().InstancePerLifetimeScope();
            builder.RegisterType<RepositoryLocator>().As<IRepositoryLocator>().InstancePerLifetimeScope();
            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<ReadOnlyUnitOfWork>().As<IReadOnlyUnitOfWork>();

            builder.RegisterType<BaseRepository<Album, MusicStoreContext>>().As<IRepository<Album>>();
            builder.RegisterType<BaseRepository<Artist, MusicStoreContext>>().As<IRepository<Artist>>();
            builder.RegisterType<BaseRepository<CartItem, MusicStoreContext>>().As<IRepository<CartItem>>();
            builder.RegisterType<BaseRepository<Genre, MusicStoreContext>>().As<IRepository<Genre>>();
            builder.RegisterType<BaseRepository<Order, MusicStoreContext>>().As<IRepository<Order>>();
            builder.RegisterType<BaseRepository<OrderDetail, MusicStoreContext>>().As<IRepository<OrderDetail>>();
        }
    }
}
