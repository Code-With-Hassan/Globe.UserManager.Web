namespace Globe.Shared.RestCallManager.Services.TokenManagerService
{
    public interface ITokenManager
    {
        string GetTokenFromCookies();
        void SaveTokenToCookies(string token);
        string ValidateToken(string token);
    }
}
