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
        public double Price { get; set; }
        public double Quantity { get; set; }
        public CryptoAction Action { get; set; }
        public string Note { get; set; }

        public int CryptoWalletId { get; set; }
        public CryptoWallet CryptoWallet { get; set; }
    }
}
