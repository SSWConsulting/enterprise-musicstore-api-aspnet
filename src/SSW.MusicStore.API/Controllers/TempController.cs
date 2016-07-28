using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SSW.MusicStore.Data;
using Microsoft.EntityFrameworkCore;
using SSW.DataOnion.Interfaces;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SSW.MusicStore.API.Controllers
{
    [Route("[controller]")]
    public class TempController : Controller
    {
        private readonly IDbContextScopeFactory _factory;

        public TempController(IDbContextScopeFactory factory)
        {
            _factory = factory;
        }

        [Route("updatedb")]
        public IActionResult updatedb()
        {
            using (var db = _factory.Create())
            {
                db.DbContexts.Get<MusicStoreContext>().Database.Migrate();
            }

            return Ok("Database Updated");
        }
    }
}
