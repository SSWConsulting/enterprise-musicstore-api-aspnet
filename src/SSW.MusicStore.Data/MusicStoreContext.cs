using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Extensions.Configuration;
using SSW.MusicStore.Data.Entities;

namespace SSW.MusicStore.Data
{
  

	public class MusicStoreContext : DbContext
    {

        /// <summary>
        /// due to stupidity , this constuctor has to go first
        /// </summary>
        /// <param name="options"></param>

        public MusicStoreContext(DbContextOptions options) : base(options)
        {
        }



        #region config for EF7 migrations (where we have little control over how resources are newed up)

        public IConfigurationRoot Configuration { get; set; }


        public MusicStoreContext()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("config.json");
            Configuration = builder.Build();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Configuration != null)
            {
                optionsBuilder.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]);
            }
            else
            {
                base.OnConfiguring(optionsBuilder);
            }
        }

        #endregion
        



        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

	    protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CartItem>().HasKey(b => b.CartItemId);

            // TODO: Remove when explicit values insertion removed.
            builder.Entity<Artist>().Property(a => a.ArtistId).ValueGeneratedNever();
            builder.Entity<Genre>().Property(g => g.GenreId).ValueGeneratedNever();

            //Deleting an album fails with this relation
            builder.Entity<Album>().Ignore(a => a.OrderDetails);
            builder.Entity<OrderDetail>().Ignore(od => od.Album);

			base.OnModelCreating(builder);
        }


	    
    }
}