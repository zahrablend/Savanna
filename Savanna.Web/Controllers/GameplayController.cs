using Microsoft.AspNetCore.Mvc;

namespace Savanna.Web.Controllers
{
    public class GameplayController : Controller
    {
        [Route("gameplay/index")]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
