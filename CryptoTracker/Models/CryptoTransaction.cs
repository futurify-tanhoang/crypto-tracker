using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class CryptoTransaction
    {
        public int Id { get; set; }
        public double BeforeBalance { get; set; }
        public double Amount { get; set; }
        public TransactionAction Action { get; set; }

        public int CryptoWalletId { get; set; }
        public CryptoWallet Wallet { get; set; }
    }
}
