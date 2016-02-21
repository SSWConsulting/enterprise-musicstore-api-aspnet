using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Autofac.Features.OwnedInstances;

using Microsoft.Data.Entity;
using SSW.MusicStore.API.Models;
using SSW.MusicStore.Data.Interfaces;

namespace SSW.MusicStore.API.Services.Query
{
    public class AlbumQueryService : IAlbumQueryService
    {
        private readonly Func<Owned<IReadOnlyUnitOfWork>> unitOfWorkFunc;

        public AlbumQueryService(Func<Owned<IReadOnlyUnitOfWork>> unitOfWorkFunc)
        {
            this.unitOfWorkFunc = unitOfWorkFunc;
        }

        public async Task<IEnumerable<Album>> GetByGenre(string genre)
        {
            using (var unitOfWork = this.unitOfWorkFunc())
            {
                var albums =
                    await unitOfWork.Value.Repository<Album>().Get().Where(a => a.Genre.Name == genre).ToListAsync();
                return albums;
            }
        }

        public async Task<IEnumerable<Album>> GetTopSellingAlbums(int count)
        {
            using (var unitOfWork = this.unitOfWorkFunc())
            {
                var albums =
                    await unitOfWork.Value.Repository<Album>().Get().OrderByDescending(a => a.OrderDetails.Count)
                        .Take(count)
                        .ToListAsync();
                return albums;
            }
        }

        public async Task<Album> GetAlbumDetails(int id)
        {
            using (var unitOfWork = this.unitOfWorkFunc())
            {
                var album =
                    await unitOfWork.Value.Repository<Album>().Get().SingleOrDefaultAsync(a => a.AlbumId == id);
                return album;
            }
        }
    }

}
