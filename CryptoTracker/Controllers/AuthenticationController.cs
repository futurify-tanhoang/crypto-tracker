using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CryptoTracker.ServiceInterfaces;
using CryptoTracker.Authentication;
using Microsoft.Extensions.Options;
using CryptoTracker.Models.ViewModels;
using CryptoTracker.Models.BindingModels;
using CryptoTracker.Helpers;
using CryptoTracker.Resources;
using Microsoft.AspNetCore.Authorization;

namespace CryptoTracker.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private IAuthenticationService _authService;
        private JwtTokenOptions _jwtTokenOptions;
        private IAccountService _accountService;

        public AuthenticationController(IAuthenticationService authService, IOptions<JwtTokenOptions> jwtTokenOptions, IAccountService accountService)
        {
            _authService = authService;
            _jwtTokenOptions = jwtTokenOptions.Value;
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<LoginResponseModel> Login([FromBody]LoginRequestModel request)
        {
            if (request == null || !ModelState.IsValid)
            {
                throw new CustomException(Errors.INVALID_INPUT);
            }

            var loginResult = await _authService.LoginAsync(request.UserName, request.Password);

            return new LoginResponseModel
            {
                AccessToken = loginResult.AccessToken,
                Expires = loginResult.ExpiredAt
            };
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task Logout()
        {
            await _authService.LogoutAsync(User);
        }

        [Authorize]
        [HttpGet("permissions")]
        public async Task<string[]> Permission()
        {
            return await _accountService.GetPermissionsOfAccountAsync(User.GetAccountId().Value);
        }
    }
}