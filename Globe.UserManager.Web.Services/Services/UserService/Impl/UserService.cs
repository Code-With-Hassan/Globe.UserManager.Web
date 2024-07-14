using Globe.Shared.Entities;
using Globe.Shared.Models.ResponseDTOs;
using Globe.Shared.Models;
using Globe.Shared.RestCallManager.Models;
using Globe.Shared.RestCallManager.Services.RestClientManager;
using Globe.UserManager.Web.Services.Repositories;
using Microsoft.Extensions.Logging;
using Globe.UserManager.Web.Services.Proxies;

namespace Globe.UserManager.Web.Services.Services.UserService.Impl
{
    public class UserService : IUserService
    {

        private readonly ILogger<UserService> _logger;
        private readonly IRestClientManager _restClientManager;
        public UserService(ILogger<UserService> logger, IRestClientManager restClientManager)
        {

            _logger = logger;
            _restClientManager = restClientManager;
        }

        public async Task<UsersListResponse> GetAllUsers()
        {
            try
            {
                Response<UsersListResponse> response = await _restClientManager.GetAsync<UsersListResponse>(UserProxy.GetAllUsers);

                return response.Data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }
    }
}
