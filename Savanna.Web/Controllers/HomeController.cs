using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Savanna.Web.Models;
using System.Diagnostics;

namespace Savanna.Web.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        [Route("home/index")]
        [Route("/")]
        public IActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"].ToString();
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
