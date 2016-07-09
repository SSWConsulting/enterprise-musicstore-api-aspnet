using Microsoft.AspNetCore.Mvc;

namespace SSW.MusicStore.API.Controllers
{
    public class HomeController : Controller
    {
		public  IActionResult Index()
        {     
            return Redirect("~/swagger/ui");
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }

        public IActionResult StatusCodePage()
        {
            return View("~/Views/Shared/StatusCodePage.cshtml");
        }

        public IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }

    }
}