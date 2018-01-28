using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class AccountRole
    {
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        [Required]
        [ForeignKey("AccountId")]
        public Account Account { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        [Required]
        public Role Role { get; set; }
    }
}
