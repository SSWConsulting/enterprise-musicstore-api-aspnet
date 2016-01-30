using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using SSW.MusicStore.DependencyResolution;

namespace SSW.MusicStore.Data.Test.Int.Setup
{
    public class Ioc
    {

        public static ILifetimeScope CreateIocScope()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(
                new DataModule(
                   // "Server=(localdb)\\mssqllocaldb;Database=SSW.MusicStore.Test;Trusted_Connection=True;MultipleActiveResultSets=true"));
                   "data source=.;initial catalog=SSW_MusicStore_Test;integrated security=True;MultipleActiveResultSets=True"));

            return builder.Build();
        }

    }
}
