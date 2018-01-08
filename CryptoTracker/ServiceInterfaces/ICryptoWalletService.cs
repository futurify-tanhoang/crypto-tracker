using CryptoTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.ServiceInterfaces
{
    public interface ICryptoWalletService
    {
        Task<CryptoWallet> GetAsync(int id);
        Task AddCryptoWalletAsync(int walletId, CryptoWallet cryptoWallet);
        Task<double> BuyAsync(int id, double amount, string note);
        Task<double> SellAsync(int id, double amount, string note);
        Task<double> WithdrawAsync(int id, int walletId, double amount);
    }
}
