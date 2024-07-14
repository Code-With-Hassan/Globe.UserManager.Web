using Microsoft.AspNetCore.Mvc;

namespace Globe.UserManager.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
