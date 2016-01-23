using System;
using System.Data.Common;

using Microsoft.Data.Entity;

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

        private readonly DbConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactory"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public DbContextFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextFactory"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public DbContextFactory(DbConnection connection)
        {
            this.connection = connection;
        }

        /// <summary>
        /// Creates new instance of DbContext using specified initializer.
        /// </summary>
        /// <returns></returns>
        public virtual TDbContext Create<TDbContext>() where TDbContext : DbContext
        {
            this.logger.Debug(
                "Creating new dbContext with connection string {connectionString}",
                this.connection == null ? this.connectionString : this.connection.ConnectionString);
            var optionsBuilder = new DbContextOptionsBuilder();
            if (this.connection == null)
            {
                optionsBuilder.UseSqlServer(this.connectionString);
            }
            else
            {
                optionsBuilder.UseSqlServer(this.connection);
            }

            var args = optionsBuilder;

            var dbContext = (TDbContext)Activator.CreateInstance(typeof(TDbContext), args);
            dbContext.Database.EnsureCreated();

            return dbContext;
        }
    }
}
