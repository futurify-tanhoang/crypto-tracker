using CryptoTracker.Models;
using CryptoTracker.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Services
{
    public class PermissionService: IPermissionService
    {
        AppDbContext _db;

        public PermissionService(AppDbContext context)
        {
            _db = context;
        }

        public async Task<IList<Permission>> GetAllAsync()
        {
            return await _db.Permissions.ToListAsync();
        }

        public async Task SyncPermissionsAsync(IEnumerable<Permission> permissions)
        {
            var allPermissions = await this.GetAllAsync();

            var syncCodes = permissions.Select(p => p.Id).ToList();

            var unchangedPermissions = allPermissions.Where(p => syncCodes.Contains(p.Id));
            var deletePermissions = allPermissions.Except(unchangedPermissions).ToList();
            var addPermissions = permissions.Where(p => !unchangedPermissions.Any(u => u.Id == p.Id)).ToList();

            if (deletePermissions.Any())
            {
                _db.RolesPermissions.RemoveRange(_db.RolesPermissions.Where(r => deletePermissions.Any(d => d.Id == r.PermissionId)));
                _db.Permissions.RemoveRange(deletePermissions);
            }

            if (addPermissions.Any())
            {
                _db.Permissions.AddRange(addPermissions);
            }

            await _db.SaveChangesAsync();
        }
    }
}
