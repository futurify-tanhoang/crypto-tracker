using CryptoTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CryptoTracker.ServiceInterfaces
{
    public interface IAuthenticationService
    {
        Task<(string AccessToken, DateTime ExpiredAt, Account Identity)> LoginAsync(string userName, string password);
        Task LogoutAsync(ClaimsPrincipal user);
        Task<bool> ValidateTokenAsync(ClaimsPrincipal user);
    }
}
