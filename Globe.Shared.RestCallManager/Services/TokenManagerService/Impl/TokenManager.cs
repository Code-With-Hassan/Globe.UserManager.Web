using Globe.Shared.RestCallManager.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Globe.Shared.RestCallManager.Services.TokenManagerService.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenManager : ITokenManager
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <param name="configuration"></param>
        public TokenManager(IHttpContextAccessor httpContextAccessor, 
            IConfiguration configuration)
        {
            _configuration = configuration;
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

        public string ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);

                // Corrected access to the validatedToken
                var jwtToken = (JwtSecurityToken)validatedToken;
                var jku = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
                var userName = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Name).Value;

                return userName;
            }
            catch
            {
                return null;
            }
        }

    }
}
