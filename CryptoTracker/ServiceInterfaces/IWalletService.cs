using CryptoTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.ServiceInterfaces
{
    public interface IWalletService
    {
        Task<Wallet> GetAsync(int id);
        Task<double> DepositAsync(int walletId, double amount, string note);
        Task<double> WithdrawAsync(int walletId, double amount, string note);
        Task<double> TransferToCryptoAsync(int walletId, int cryptoWalletId, double amount);
    }
}
