using Microsoft.AspNetCore.Mvc;

namespace Fomo.Api.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
