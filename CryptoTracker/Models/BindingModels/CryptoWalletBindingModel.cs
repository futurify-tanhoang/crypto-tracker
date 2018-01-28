using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Models.BindingModels
{
    public class CryptoWalletBindingModel
    {
        [Required]
        public int CryptoCurrencyId { get; set; }
    }

    public class BuyCryptoBindingModel
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public string Note { get; set; }
    }

    public class SellCryptoBindingModel: BuyCryptoBindingModel
    {
    }

    public class WithdrawCryptoBindingModel
    {
        public int CryptoWalletId { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
    }
}
