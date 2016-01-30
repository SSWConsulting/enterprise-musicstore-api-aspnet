using System;
using System.CodeDom;
using System.Data;
using System.Data.Common;

using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Storage.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog;

using SSW.MusicStore.Data.Interfaces;

namespace SSW.MusicStore.Data
{
    /// <summary>
    /// Factory for creating new instance of DbContext. Uses initializer and connection string to create new
    /// database instance
    /// </summary>
    public class DbContextFactory : IDbContextFactory
    {
        private readonly ILogger logger = Log.ForContext<DbContextFactory>();

        private readonly string connectionString;


        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DbContextFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }


        /// <summary>
        /// Creates new instance of DbContext using specified initializer.
        /// </summary>
        /// <returns></returns>
        public virtual TDbContext Create<TDbContext>() where TDbContext : DbContext
        {
            this.logger.Debug(
                "Creating new dbContext with connection string {connectionString}",this.connectionString);

            //var optionsBuilder = new DbContextOptionsBuilder();
            //optionsBuilder.UseSqlServer(this.connectionString);


            // create serviceProvider
            var serviceProvider = new ServiceCollection()
                .AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<TDbContext>(options =>
                {
                    options.UseSqlServer(this.connectionString);
                })
                .GetInfrastructure()

                .Replace(new ServiceDescriptor(typeof(SqlServerDatabaseCreator), typeof(SqlDbCreator), ServiceLifetime.Scoped))
                .BuildServiceProvider();


            var dbContext = serviceProvider.GetService<TDbContext>();

 //           var dbContext = (TDbContext)Activator.CreateInstance(typeof(TDbContext), args);

            logger.Information("user: {user}", System.Environment.UserName);

            dbContext.Database.Migrate();

            return dbContext;
        }
    }
}
