using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class Account: BaseObject
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsVerified { get; set; }
        public bool IsSystemAdmin { get; set; }
        public string Password { get; set; }

        public List<AccountRole> Roles { get; set; }
        public List<AccountPermission> Permissions { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }
}
