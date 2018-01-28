using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models.ViewModels
{
    public class CryptoWalletViewModel
    {
        public int Id { get; set; }
        public double Balance { get; set; }
        public string CryptoName { get; set; }
    }
}
