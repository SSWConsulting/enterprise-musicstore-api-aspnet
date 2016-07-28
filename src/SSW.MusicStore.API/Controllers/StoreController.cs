using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using SSW.MusicStore.BusinessLogic.Interfaces.Query;
using SSW.MusicStore.Data.Entities;

namespace SSW.MusicStore.API.Controllers
{
    //[Authorize]
    [Route("api")]
    public class StoreController : Controller
    {
        private readonly IGenreQueryService _genreQueryService;
        private readonly IAlbumQueryService _albumQueryService;

        public StoreController(
            IGenreQueryService genreQueryService,
            IAlbumQueryService albumQueryService)
        {
            _genreQueryService = genreQueryService;
            _albumQueryService = albumQueryService;
        }

        [HttpGet("genres")]
        public async Task<ActionResult> Get()
        {
            var results = await _genreQueryService.GetAllGenres();
            if (results == null || !results.Any())
            {
                return this.NotFound();
            }

            return Json(results);
        }

        [HttpGet("albums/{genre}")]
        public async Task<ActionResult> Get(string genre)
        {
            var results = await _albumQueryService.GetByGenre(genre);
            if (!results.Any())
            {
                return NotFound();
            }

            return Json(results);
        }


        [HttpGet("albums/details/{id}")]
        public async Task<ActionResult> Details(int id)
        {
            var album = await _albumQueryService.GetAlbumDetails(id);
            if (album != null) return new JsonResult(album);

            Log.Logger.Warning("User tried to retrieve album with {id} which doesn't exist", id);
            return NotFound();
        }

        [HttpGet("popular")]
        public async Task<JsonResult> Popular()
        {
            var albums = await GetTopSellingAlbumsAsync(6);
            return Json(albums);
        }

        [HttpGet]
        [Authorize(ActiveAuthenticationSchemes = "Bearer")]
        [Route("secured/ping")]
        public object SecuredPing()
        {
            return new
            {
                message = "Pong. You accessed a protected endpoint.",
                claims = User.Claims.Select(c => new { c.Type, c.Value })
            };
        }

        private async Task<IEnumerable<Album>> GetTopSellingAlbumsAsync(int count)
        {
            return await _albumQueryService.GetTopSellingAlbums(count);
        }
    }
}
