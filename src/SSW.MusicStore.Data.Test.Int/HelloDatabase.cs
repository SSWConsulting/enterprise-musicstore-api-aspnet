using System.Linq;

using Autofac;
using SSW.MusicStore.Data.Entities;
using SSW.MusicStore.Data.Interfaces;
using Xunit;

namespace SSW.MusicStore.Data.Test.Int
{
    public class HelloDatabase
    {
        private readonly ILifetimeScope container;

        public HelloDatabase()
        {
            this.container = Setup.Ioc.CreateIocScope();
            Setup.Logging.SerilogConfiguration();
        }

        [Fact]
        public void HelloDatabaseTest()
        {
            var dbScopeFactory = this.container.Resolve<IDbContextScopeFactory>();

            using (var dbScope = dbScopeFactory.Create())
            using (var dbContext = dbScope.DbContexts.Get<MusicStoreContext>())
            {
                var countBefore = dbContext.Genres.Count();

                dbContext.Genres.Add(new Genre()
                {
                    Description = "My Genre Description",
                    Name = "My Genre"
                });
                dbScope.SaveChanges();

                Assert.True(dbContext.Genres.Count() == countBefore +1);
            }
        }
    }
}
