using CryptoTracker.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTracker.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using CryptoTracker.Resources;
using Microsoft.AspNetCore.Identity;
using CryptoTracker.Helpers;
using CryptoTracker.Authentication;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace CryptoTracker.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        AppDbContext _context;
        private JwtTokenOptions _jwtTokenOptions;

        public AuthenticationService(AppDbContext context, IOptions<JwtTokenOptions> jwtTokenOptions)
        {
            _context = context;
            _jwtTokenOptions = jwtTokenOptions.Value;
        }

        public async Task<(string AccessToken, DateTime ExpiredAt, Account Identity)> LoginAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Email == userName);
            if (account == null)
            {
                throw new CustomException(Errors.INCORRECT_LOGIN);
            }

            var passwordHasher = new PasswordHasher<Account>();

            var verifyResult = passwordHasher.VerifyHashedPassword(account, account.Password, password);

            switch (verifyResult)
            {
                case PasswordVerificationResult.Failed:
                    throw new CustomException(Errors.INCORRECT_LOGIN);
                case PasswordVerificationResult.Success:
                    break;
                case PasswordVerificationResult.SuccessRehashNeeded:
                    account.Password = passwordHasher.HashPassword(account, password);
                    break;
                default:
                    throw new CustomException(Errors.INCORRECT_LOGIN);

            }

            if (!account.IsVerified)
            {
                throw new CustomException(Errors.ACCOUNT_HAS_NOT_VERIFIED);
            }

            var loginRecord = new LoginRecord
            {
                AccountId = account.Id
            };

            _context.LoginRecords.Add(loginRecord);

            await _context.SaveChangesAsync();

            var issuedTime = DateTime.UtcNow;
            var expiration = (int)_jwtTokenOptions.Expiration.TotalSeconds;
            var expiredAt = DateTime.Now.AddSeconds(expiration);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                new Claim(JwtRegisteredClaimNames.Jti, loginRecord.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, (issuedTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds.ToString(), ClaimValueTypes.Integer64),
                new Claim("USER:ID", account.Id.ToString())
            };

            var jwt = new JwtSecurityToken(
                issuer: _jwtTokenOptions.Issuer,
                audience: _jwtTokenOptions.Audience,
                claims: claims,
                notBefore: issuedTime,
                expires: issuedTime.Add(_jwtTokenOptions.Expiration),
                signingCredentials: _jwtTokenOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return (encodedJwt, expiredAt, account);
        }

        public async Task LogoutAsync(ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            int? accountId = user.GetAccountId();

            if (!accountId.HasValue)
            {
                return;
            }

            var jtiStr = user.FindFirstValue(JwtRegisteredClaimNames.Jti);
            if (!Guid.TryParse(jtiStr, out Guid jti))
            {
                throw new CustomException(Errors.INVALID_REQUEST);
            }

            var loginRecord = await _context.LoginRecords.FirstOrDefaultAsync(l => l.AccountId == accountId && l.Id == jti);

            if (loginRecord == null)
            {
                throw new CustomException(Errors.INVALID_REQUEST);
            }

            loginRecord.Revoked = true;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateTokenAsync(ClaimsPrincipal user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            int? accountId = user.GetAccountId();

            if (!accountId.HasValue)
            {
                throw new CustomException(Errors.INVALID_REQUEST);
            }

            var jtiStr = user.FindFirstValue(JwtRegisteredClaimNames.Jti);
            if (!Guid.TryParse(jtiStr, out Guid jti))
            {
                throw new CustomException(Errors.INVALID_REQUEST);
            }

            var loginRecord = await _context.LoginRecords.FirstOrDefaultAsync(l => l.AccountId == accountId && l.Id == jti);

            if (loginRecord == null)
            {
                throw new CustomException(Errors.INVALID_REQUEST);
            }

            return !loginRecord.Revoked;
        }
    }
}
