using CryptoTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Setup
{
    public static class ConfigureSystemAdminTask
    {
        public static void ConfigureSystemAdmin(this IServiceProvider _services)
        {
            var _db = _services.GetRequiredService<AppDbContext>();

            var sysadmin = _db.Accounts.Include(a => a.Permissions).FirstOrDefault(a => a.Email == "tanhn90@gmail.com");

            if (sysadmin == null)
            {
                sysadmin = new Account
                {
                    Email = "tanhn90@gmail.com",
                    Permissions = _db.Permissions.Select(p => new AccountPermission { PermissionId = p.Id }).ToList(),
                    Wallet = new Wallet ()
                };

                var _pwdHasher = new PasswordHasher<Account>();

                var hashedPassword = _pwdHasher.HashPassword(sysadmin, "Rain@sym");

                sysadmin.Password = hashedPassword;

                sysadmin.IsSystemAdmin = true;
                sysadmin.IsVerified = true;

                var now = DateTime.Now;

                sysadmin.CreatedAt = now;
                sysadmin.ModifiedAt = now;

                _db.Accounts.Add(sysadmin);
            }
            else
            {
                var hadPermissions = _db.AccountsPermissions.Where(a => a.AccountId == sysadmin.Id).Select(a => a.PermissionId);
                var missingPermissions = _db.Permissions.Where(p => !hadPermissions.Contains(p.Id)).Select(p => p.Id);
                sysadmin.Permissions.AddRange(missingPermissions.Select(p => new AccountPermission { AccountId = sysadmin.Id, PermissionId = p }));
            }
            _db.SaveChanges();
        }
    }
}
