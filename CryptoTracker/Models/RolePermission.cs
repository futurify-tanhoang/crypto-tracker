using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class RolePermission: BaseObject
    {
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [Required]
        [ForeignKey("RoleId")]
        public Role Role { get; set; }

        [ForeignKey("Permission")]
        public string PermissionId { get; set; }
        [Required]
        [ForeignKey("PermissionId")]
        public Permission Permission { get; set; }
    }
}
