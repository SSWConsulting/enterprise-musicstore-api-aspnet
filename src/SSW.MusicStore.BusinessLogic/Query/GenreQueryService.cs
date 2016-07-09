using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.OwnedInstances;
using Microsoft.EntityFrameworkCore;
using SSW.DataOnion.Interfaces;
using SSW.MusicStore.BusinessLogic.Interfaces.Query;
using SSW.MusicStore.Data.Entities;

namespace SSW.MusicStore.BusinessLogic.Query
{
	public class GenreQueryService : IGenreQueryService
	{
        private readonly Func<Owned<IReadOnlyUnitOfWork>> unitOfWorkFunc;

        public GenreQueryService(Func<Owned<IReadOnlyUnitOfWork>> unitOfWorkFunc)
        {
            this.unitOfWorkFunc = unitOfWorkFunc;
        }

        public async Task<IEnumerable<Genre>> GetAllGenres()
		{
			using (var unitOfWork = this.unitOfWorkFunc())
			{
			    var genres = await unitOfWork.Value.Repository<Genre>().Get().ToListAsync();
			    return genres;
			}
		}
	}

}
