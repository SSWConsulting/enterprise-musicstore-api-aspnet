using System;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using SSW.MusicStore.API.Models;

namespace SSW.MusicStore.API.Tests.Integration.Setup
{
    public static class ServiceProviderFactory
    {
		public static IServiceProvider Create()
		{
			var services = new ServiceCollection();

			services.AddEntityFramework()
					  .AddInMemoryDatabase()
					  .AddDbContext<MusicStoreContext>(options => options.UseInMemoryDatabase());

			var serviceProvider = services.BuildServiceProvider();
			return serviceProvider;
        }
    }
}
