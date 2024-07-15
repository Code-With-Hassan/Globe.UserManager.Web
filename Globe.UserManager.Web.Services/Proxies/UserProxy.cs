using Globe.Shared.RestCallManager.Models;

namespace Globe.UserManager.Web.Services.Proxies
{
    public record GetAllUsers(string Url = "Users/", bool IsPublic = true) : IProxy;

    public class UserProxy
    {
        //public static string GetAllUsers = "Users/";

    }
}
