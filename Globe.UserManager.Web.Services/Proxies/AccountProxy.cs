using Globe.Shared.RestCallManager.Models;

namespace Globe.UserManager.Web.Services.Repositories
{
    public record LoginAsync(string Url = "v1/login", bool IsPublic = false) : IProxy;

}
