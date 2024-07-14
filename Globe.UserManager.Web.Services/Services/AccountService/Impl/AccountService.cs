﻿using Microsoft.Extensions.Logging;
using Globe.Shared.RestCallManager.Models;
using Globe.Shared.RestCallManager.Services.RestClientManager;
using Globe.Shared.Models;
using Globe.Shared.Models.ResponseDTOs;
using Globe.UserManager.Web.Services.Repositories;

namespace Globe.UserManager.Web.Services.Services.AccountService.Impl
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountService> _logger;
        private readonly IRestClientManager _restClientManager;
        public AccountService(ILogger<AccountService> logger, IRestClientManager restClientManager)
        {

            _logger = logger;
            _restClientManager = restClientManager;
        }
        public async Task<Response<LoginDTO>> LoginAsync(LoginModel model)
        {
            try
            {
                return await _restClientManager.PostAsync<LoginDTO, LoginModel>(AccountProxy.Login, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}