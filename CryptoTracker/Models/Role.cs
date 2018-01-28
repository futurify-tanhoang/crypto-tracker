using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class Role: BaseObject
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public List<RolePermission> Permissions { get; set; }
    }
}
