using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models
{
    public class Wallet: BaseObject
    {
        public int Id { get; set; }
        public double Balance { get; set; }

        public List<CryptoWallet> Cryptos { get; set; }
    }
}
