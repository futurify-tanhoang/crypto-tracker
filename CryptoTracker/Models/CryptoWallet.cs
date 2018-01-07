using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class CryptoWallet: BaseObject
    {
        public int Id { get; set; }
        public double Balance { get; set; }

        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public int CryptoCurrencyId { get; set; }
        public CryptoCurrency CryptoCurrency { get; set; }
    }
}
