using Globe.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Globe.UserManager.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet("/Login")]
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                if (loginModel.Password.Equals("Test", StringComparison.InvariantCultureIgnoreCase) &&
                    loginModel.Username.Equals("Test", StringComparison.InvariantCultureIgnoreCase))
                    return RedirectToAction(nameof(Index), "Home");
            }
            return BadRequest();
        }
    }
}
