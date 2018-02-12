using CryptoTracker.Authentication;
using CryptoTracker.Exceptions;
using CryptoTracker.Models.BindingModels;
using CryptoTracker.Models.ViewModels;
using CryptoTracker.Resources;
using CryptoTracker.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

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