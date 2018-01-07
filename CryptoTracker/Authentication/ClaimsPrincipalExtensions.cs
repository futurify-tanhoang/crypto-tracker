using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CryptoTracker.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static int? GetAccountId(this ClaimsPrincipal user)
        {
            if (user == null)
            {
                return null;
            }

            if (user.Claims == null)
            {
                return null;
            }

            string userId = user.FindFirstValue("USER:ID");

            if (int.TryParse(userId, out int id))
            {
                return id;
            }
            else
            {
                return null;
            }
        }
    }
}
