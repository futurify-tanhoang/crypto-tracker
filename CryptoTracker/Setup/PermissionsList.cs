using CryptoTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Setup
{
    public static class PermissionsList
    {
        static PermissionsList()
        {
            GroupsPermissions = new PermissionsGroup[]
            {
                new PermissionsGroup
                {
                    Name = "PERMISSIONS.MANAGE_ROLES.GROUP_TITLE",
                    Permissions = new Permission[]
                    {
                        new Permission(PermissionConstants.VIEW_ROLES, "PERMISSIONS.MANAGE_ROLES.VIEW_ROLES"),
                        new Permission(PermissionConstants.MODIFY_ROLE, "PERMISSIONS.MANAGE_ROLES.MODIFY_ROLE"),
                        new Permission(PermissionConstants.REMOVE_ROLE, "PERMISSIONS.MANAGE_ROLES.REMOVE_ROLE")
                    }
                }
            };
        }

        public static PermissionsGroup[] GroupsPermissions { get; }

    }
}
