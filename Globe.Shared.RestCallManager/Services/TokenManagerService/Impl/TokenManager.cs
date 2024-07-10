using Globe.Shared.RestCallManager.Constants;
using Microsoft.AspNetCore.Http;

namespace Globe.Shared.RestCallManager.Services.TokenManagerService.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenManager : ITokenManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public TokenManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTokenFromCookies()
        {
            string token = string.Empty;
            var cookies = _httpContextAccessor.HttpContext.Request.Cookies;

            cookies.TryGetValue(SharedConstants.JWT_Token, out token);

            return token;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public void SaveTokenToCookies(string token)
        {
            var cookies = _httpContextAccessor.HttpContext.Response.Cookies;
            cookies.Append(SharedConstants.JWT_Token, token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(1)
            });
        }
    }
}
