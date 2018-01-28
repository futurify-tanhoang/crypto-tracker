using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class Permission
    {
        public Permission() { }

        public Permission(string id, string display)
        {
            this.Id = id;
            this.Display = display;
        }

        [Key]
        public string Id { get; set; }
        public string Display { get; set; }
    }

    public class AccountPermission
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public string PermissionId { get; set; }
        public Permission Permission { get; set; }
    }
}
