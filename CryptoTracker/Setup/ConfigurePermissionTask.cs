using CryptoTracker.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Setup
{
    public static class ConfigurePermissionTask
    {

        public static void ConfigurePermissions(this IServiceProvider _services)
        {
            var permissionService = _services.GetRequiredService<IPermissionService>();

            permissionService.SyncPermissionsAsync(PermissionsList.GroupsPermissions.SelectMany(p => p.Permissions)).Wait();
        }

    }
}
