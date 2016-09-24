using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSW.DataOnion.Core;
using SSW.DataOnion.Core.Initializers;
using SSW.DataOnion.DependencyResolution.Autofac;
using SSW.DataOnion.Interfaces;
using SSW.MusicStore.API.Services;
using SSW.MusicStore.BusinessLogic.Command;
using SSW.MusicStore.BusinessLogic.Interfaces.Command;
using SSW.MusicStore.Data;
using SSW.MusicStore.Data.Entities;
using System.Reflection;

namespace SSW.MusicStore.API.Infrastructure.DI
{
    public static class IoC
    {
        public static ILifetimeScope CreateAutofacContainer(this IServiceCollection services, IConfigurationRoot configuration)
        {
            var builder = new ContainerBuilder();

            // Load web specific dependencies
            builder.RegisterType<AuthMessageSender>()
                .As<IEmailSender>().InstancePerLifetimeScope();
            builder.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof (CartCommandService).GetTypeInfo().Assembly, typeof (ICartCommandService).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            var databaseInitializer = new MigrateToLatestVersion(new SampleDataSeeder());
            builder.AddDataOnion(new DbContextConfig(configuration.GetConnectionString("DefaultConnection"), typeof(MusicStoreContext), databaseInitializer));

            // Populate the container with services that were previously registered
            builder.Populate(services);

            builder.RegisterType<BaseRepository<Album, MusicStoreContext>>().As<IRepository<Album>>();
            builder.RegisterType<BaseRepository<Artist, MusicStoreContext>>().As<IRepository<Artist>>();
            builder.RegisterType<BaseRepository<Cart, MusicStoreContext>>().As<IRepository<Cart>>();
            builder.RegisterType<BaseRepository<CartItem, MusicStoreContext>>().As<IRepository<CartItem>>();
            builder.RegisterType<BaseRepository<Genre, MusicStoreContext>>().As<IRepository<Genre>>();
            builder.RegisterType<BaseRepository<Order, MusicStoreContext>>().As<IRepository<Order>>();
            builder.RegisterType<BaseRepository<OrderDetail, MusicStoreContext>>().As<IRepository<OrderDetail>>();

            var container = builder.Build();

            return container;
        }
    }
}
