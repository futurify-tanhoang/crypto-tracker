using CryptoTracker.Exceptions;
using CryptoTracker.Models;
using CryptoTracker.Resources;
using CryptoTracker.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Services
{
    public class WalletService : IWalletService
    {
        private AppDbContext _context;

        public WalletService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> GetAsync(int id)
        {
            return await _context.Wallets.Include(s => s.Cryptos).ThenInclude(s => s.CryptoCurrency).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<double> DepositAsync(int walletId, double amount, string note)
        {
            var wallet = await GetAsync(walletId);
            if (wallet == null)
            {
                throw new CustomException(Errors.WALLET_NOT_FOUND, Errors.WALLET_NOT_FOUND_MSG);
            }

            var beforeBanalce = wallet.Balance;
            wallet.Balance += amount;

            //add transaction history
            _context.WalletTransactions.Add(new WalletTransaction { Action = TransactionAction.Deposit, Amount = amount, BeforeBalance = beforeBanalce, WalletId = wallet.Id, Note = note });

            await _context.SaveChangesAsync();

            return wallet.Balance;
        }

        public async Task<double> TransferToCryptoAsync(int walletId, int cryptoWalletId, double price, double quantity)
        {
            var wallet = await GetAsync(walletId);
            if (wallet == null)
            {
                throw new CustomException(Errors.WALLET_NOT_FOUND, Errors.WALLET_NOT_FOUND_MSG);
            }
            var cryptoWallet = wallet.Cryptos.FirstOrDefault(t => t.Id == cryptoWalletId);
            if (cryptoWallet == null)
            {
                throw new CustomException(Errors.WALLET_NOT_FOUND, Errors.WALLET_NOT_FOUND_MSG);
            }

            var amount = price * quantity;
            //update balance of wallet
            var beforeBanalce = wallet.Balance;
            wallet.Balance -= amount;

            //add wallet transaction history
            _context.WalletTransactions.Add(new WalletTransaction { Action = TransactionAction.Transfer, Amount = amount, BeforeBalance = beforeBanalce, WalletId = wallet.Id, Note = $"Deposit for {cryptoWallet.CryptoCurrency.Name} wallet" });

            //update balance of crypto wallet
            beforeBanalce = cryptoWallet.Balance;
            cryptoWallet.Balance += quantity;

            //add crypto transaction history
            _context.CryptoTransactions.Add(new CryptoTransaction { Action = CryptoAction.Deposit, Price = price, Quantity = quantity, BeforeBalance = beforeBanalce, CryptoWalletId = cryptoWalletId, Note = "Receiving from my wallet" });

            await _context.SaveChangesAsync();

            return wallet.Balance;
        }

        public async Task<double> WithdrawAsync(int walletId, double amount, string note)
        {
            var wallet = await GetAsync(walletId);
            if (wallet == null)
            {
                throw new CustomException(Errors.WALLET_NOT_FOUND, Errors.WALLET_NOT_FOUND_MSG);
            }

            var beforeBanalce = wallet.Balance;
            wallet.Balance -= amount;

            //add transaction history
            _context.WalletTransactions.Add(new WalletTransaction { Action = TransactionAction.Withdraw, Amount = amount, BeforeBalance = beforeBanalce, WalletId = wallet.Id, Note = note });

            await _context.SaveChangesAsync();

            return wallet.Balance;
        }
    }
}
