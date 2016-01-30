using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using SSW.MusicStore.Data.Entities;
using SSW.MusicStore.Data.Interfaces;
using Xunit;

namespace SSW.MusicStore.Data.Test.Int
{
    public class HelloDatabase
    {
        private readonly ILifetimeScope container = null;
        public HelloDatabase()
        {
            container = Setup.Ioc.CreateIocScope();
            Setup.Logging.SerilogConfiguration();
        }

        [Fact]
        public void HelloDatabaseTest()
        {
            var dbScopeFactory = container.Resolve<IDbContextScopeFactory>();

            using (var dbScope = dbScopeFactory.Create())
            using (var dbContext = dbScope.DbContexts.Get<MusicStoreContext>())
            {
                int countBefore = dbContext.Genres.Count();

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
