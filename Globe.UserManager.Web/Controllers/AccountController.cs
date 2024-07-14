using Globe.Shared.Models;
using Globe.Shared.Models.ResponseDTOs;
using Globe.Shared.RestCallManager.Constants;
using Globe.Shared.RestCallManager.Models;
using Globe.Shared.RestCallManager.Services.TokenManagerService;
using Globe.UserManager.Web.Services.AccountService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Globe.UserManager.Web.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly ITokenManager _tokenManager;
        private readonly IConfiguration _configuration;
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ITokenManager tokenManager,
                                IConfiguration configuration,
                                IAccountService accountService,
                                ILogger<AccountController> logger)
        {
            _logger = logger;
            _tokenManager = tokenManager;
            _configuration = configuration;
            _accountService = accountService;
        }


        [HttpGet("/Login")]
        public IActionResult Index(string? ReturnUrl)
        {
            if (User.Identity!.IsAuthenticated)
                // Redirect to the desired page after successful login
                return LocalRedirect(ReturnUrl ?? "/");
            
            ViewBag.ReturnUrl = ReturnUrl;
            
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost("/Authenticate")]
        public async Task<IActionResult> Authenticate(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Response<LoginDTO> loginResponse = await _accountService.LoginAsync(loginModel);

                    LoginDTO loginDTO = loginResponse.Data;

                    if (loginResponse.Success)
                    {
                        // Add user ID to the claims in User.Identity
                        List<Claim> claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Sid, loginDTO.User.Id),
                            new Claim(ClaimTypes.NameIdentifier, loginModel.Username),
                            new Claim(ClaimTypes.Name, loginModel.Username),
                            new Claim(SharedConstants.JWT_Token, loginDTO.Token),
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties authenticationProperties = new AuthenticationProperties
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpireMinutes"))
                        };

                        await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity), authenticationProperties);

                        // Redirect to the desired page after successful login
                        if (!string.IsNullOrWhiteSpace(loginModel.ReturnUrl))
                            return LocalRedirect(loginModel.ReturnUrl ?? "/");

                        return RedirectToAction("Index", "Dashboard");
                    }

                    ModelState.AddModelError(string.Empty, loginResponse.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(nameof(Index));
        }
    }
}
