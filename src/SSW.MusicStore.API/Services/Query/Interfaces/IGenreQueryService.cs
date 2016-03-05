using System.Collections.Generic;
using System.Threading.Tasks;

using SSW.MusicStore.Data.Entities;

namespace SSW.MusicStore.API.Services.Query.Interfaces
{
    public interface IGenreQueryService
    {
		/// <summary>
		/// Gets all genres
		/// </summary>
		/// <returns>List of genres</returns>
	    Task<IEnumerable<Genre>> GetAllGenres();
    }
}
