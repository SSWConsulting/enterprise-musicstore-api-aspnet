using Autofac;

using SSW.MusicStore.Data.Initializers;
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
                    "Server=(localdb)\\mssqllocaldb;Database=SSW.MusicStore.Test;Trusted_Connection=True;MultipleActiveResultSets=true",
                    new DropCreateDatabaseAlways(new SampleDataSeeder())));

            return builder.Build();
        }

    }
}
