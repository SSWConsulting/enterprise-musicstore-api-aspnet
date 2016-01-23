using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Data.Entity;
using Microsoft.Extensions.DependencyInjection;
using SSW.MusicStore.API.Models;
using SSW.MusicStore.Data.Interfaces;

namespace SSW.MusicStore.API.Services.Query
{
    public class AlbumQueryService : IAlbumQueryService
    {
        private readonly IDbContextFactory<MusicStoreContext> dbContextFactory;

        private readonly IDbContextScopeFactory dbContextScopeFactory;

        public AlbumQueryService(
            IDbContextFactory<MusicStoreContext> dbContextFactory,
            IDbContextScopeFactory dbContextScopeFactory)
        {
            this.dbContextFactory = dbContextFactory;
            this.dbContextScopeFactory = dbContextScopeFactory;
        }

        public async Task<IEnumerable<Album>> GetByGenreTest(string genre)
        {
            var db = this.dbContextScopeFactory.CreateReadOnly();
            var albums =
                await
                    db.DbContexts.Get<MusicStoreContext>()
                        .Albums.Where(a => a.Genre.Name == genre)
                        .ToListAsync();
            return albums;
        }

        public async Task<IEnumerable<Album>> GetByGenre(string genre)
        {
            using (var dbContext = this.dbContextFactory.Create())
            {
                var albums =
                    await dbContext.Albums
                                 .Where(a => a.Genre.Name == genre)
                                 .ToListAsync();
                return albums;
            }
        }

        public async Task<IEnumerable<Album>> GetTopSellingAlbums(int count)
        {
            using (var dbContext = this.dbContextFactory.Create())
            {
                var albums =
                    await dbContext.Albums
                        .OrderByDescending(a => a.OrderDetails.Count)
                        .Take(count)
                        .ToListAsync();
                return albums;
            }
        }

        public async Task<Album> GetAlbumDetails(int id)
        {
            using (var dbContext = this.dbContextFactory.Create())
            {
                var albums =
                    await dbContext.Albums
                        .SingleOrDefaultAsync(a => a.AlbumId == id);
                return albums;
            }
        }
    }

}
