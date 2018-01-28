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
        Task<List<CryptoWallet>> GetByWalletIdAsync(int walletId);
        Task<CryptoWallet> CreateAsync(CryptoWallet cryptoWallet);
        Task<double> BuyAsync(int id, double quantity, double price, string note);
        Task<double> SellAsync(int id, double quantity, double price, string note);
        Task<double> WithdrawAsync(int id, int walletId, double price, double quantity);
    }
}
