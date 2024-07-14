using Globe.Shared.Models;
using Globe.Shared.Models.ResponseDTOs;
using Globe.Shared.RestCallManager.Models;

namespace Globe.UserManager.Web.Services.Services.AccountService
{
    public interface IAccountService
    {
        public Task<Response<LoginDTO>> LoginAsync(LoginModel model);

    }
}
