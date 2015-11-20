using SSW.MusicStore.API.Services;

namespace SSW.MusicStore.API.Tests.Integration.Setup
{
    public static class DbContextHelper
    {
		public static DbContextFactory CreateDbContextFactory()
		{
			var dbContextFactory = new DbContextFactory(ServiceProviderFactory.Create());
			return dbContextFactory;
        }
    }
}
