using Globe.Shared.Models.ResponseDTOs;

namespace Globe.UserManager.Web.Services.Services.UserService
{
    public interface IUserService
    {
        Task<UsersListResponse> GetAllUsers();

    }
}
