using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class WalletTransaction: BaseObject
    {
        public int Id { get; set; }
        public double BeforeBalance { get; set; }
        public double Amount { get; set; }
        public TransactionAction Action { get; set; }
        public string Note { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }
    }


}
