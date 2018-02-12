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
                },
                new PermissionsGroup
                {
                    Name = "PERMISSIONS.MANAGE_CRYPTOCURRENCY",
                    Permissions = new Permission[]
                    {
                        new Permission(PermissionConstants.VIEW_CRYPTOCURRENCY, "PERMISSIONS.MANAGE_CRYPTOCURRENCY.VIEW_CRYPTOCURRENCY"),
                        new Permission(PermissionConstants.MODIFY_CRYPTOCURRENCY, "PERMISSIONS.MANAGE_CRYPTOCURRENCY.MODIFY_CRYPTOCURRENCY"),
                        new Permission(PermissionConstants.REMOVE_CRYPTOCURRENCY, "PERMISSIONS.MANAGE_CRYPTOCURRENCY.REMOVE_CRYPTOCURRENCY"),
                    }
                }
            };
        }

        public static PermissionsGroup[] GroupsPermissions { get; }

    }
}
