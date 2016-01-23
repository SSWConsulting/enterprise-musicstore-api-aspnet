using System.Reflection;

using Autofac;

using Microsoft.Data.Entity.Infrastructure;

using SSW.MusicStore.Data;
using SSW.MusicStore.Data.Interfaces;

using Module = Autofac.Module;

namespace SSW.MusicStore.DependencyResolution
{
    public class DataModule : Module
    {
        private readonly string connectionString;

        public DataModule(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterAssemblyTypes(
            //    Assembly.GetAssembly(typeof(TransactionRepository)),
            //    Assembly.GetAssembly(typeof(ITransactionRepository)))
            //    .AsClosedTypesOf(typeof(IRepository<>))
            //    .AsImplementedInterfaces();

            builder.RegisterType<DbContextFactory>()
                .As<IDbContextFactory>()
                .WithParameter("connectionString", this.connectionString);

            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>().InstancePerLifetimeScope();
            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>();
        }
    }
}
