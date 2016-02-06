using System.Reflection;

using Autofac;

using Microsoft.Data.Entity.Infrastructure;

using SSW.MusicStore.Data;
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

            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>();
        }
    }
}
