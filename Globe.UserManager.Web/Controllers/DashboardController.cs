using Microsoft.AspNetCore.Mvc;

namespace Globe.UserManager.Web.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
