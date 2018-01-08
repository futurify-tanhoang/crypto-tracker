﻿using CryptoTracker.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTracker.Models;
using Microsoft.EntityFrameworkCore;
using CryptoTracker.Helpers;
using CryptoTracker.Resources;

namespace CryptoTracker.Services
{
    public class CryptoWalletService : ICryptoWalletService
    {
        private AppDbContext _context;

        public CryptoWalletService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CryptoWallet> GetAsync(int id)
        {
            return await _context.CryptoWallets.Include(s => s.CryptoCurrency).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddCryptoWalletAsync(int walletId, CryptoWallet cryptoWallet)
        {
            var wallet = await _context.Wallets.Include(s => s.Cryptos).ThenInclude(s => s.CryptoCurrency).FirstOrDefaultAsync(t => t.Id == walletId);
            if (wallet == null)
            {
                throw new CustomException(Errors.WALLET_NOT_FOUND, Errors.WALLET_NOT_FOUND_MSG);
            }

            wallet.Cryptos = wallet.Cryptos == null ? new List<CryptoWallet>() : wallet.Cryptos;
            wallet.Cryptos.Add(cryptoWallet);

            await _context.SaveChangesAsync();
        }

        public async Task<double> BuyAsync(int id, double amount, string note)
        {
            var wallet = await GetAsync(id);
            if (wallet == null)
            {
                throw new CustomException(Errors.WALLET_NOT_FOUND, Errors.WALLET_NOT_FOUND_MSG);
            }

            //update balance
            var beforeBalance = wallet.Balance;
            wallet.Balance -= amount;

            //add transaction history
            _context.CryptoTransactions.Add(new CryptoTransaction
            {
                Action = CryptoAction.Buy,
                Amount = amount,
                BeforeBalance = beforeBalance,
                CryptoWalletId = id,
                Note = note
            });

            await _context.SaveChangesAsync();

            return wallet.Balance;
        }

        public async Task<double> SellAsync(int id, double amount, string note)
        {
            var wallet = await GetAsync(id);
            if (wallet == null)
            {
                throw new CustomException(Errors.CRYPTO_WALLET_NOT_FOUND, Errors.CRYPTO_WALLET_NOT_FOUND_MSG);
            }

            //update balance
            var beforeBalance = wallet.Balance;
            wallet.Balance += amount;

            //add transaction history
            _context.CryptoTransactions.Add(new CryptoTransaction
            {
                Action = CryptoAction.Sell,
                Amount = amount,
                BeforeBalance = beforeBalance,
                CryptoWalletId = id,
                Note = note
            });

            await _context.SaveChangesAsync();

            return wallet.Balance;
        }

        public async Task<double> WithdrawAsync(int id, int walletId, double amount)
        {
            var wallet = await _context.Wallets.Include(s => s.Cryptos).FirstOrDefaultAsync(t => t.Id == walletId);
            if (wallet == null)
            {
                throw new CustomException(Errors.WALLET_NOT_FOUND, Errors.WALLET_NOT_FOUND_MSG);
            }

            var cryptoWallet = wallet.Cryptos.FirstOrDefault(t => t.Id == id);
            if (cryptoWallet == null)
            {
                throw new CustomException(Errors.CRYPTO_WALLET_NOT_FOUND, Errors.CRYPTO_WALLET_NOT_FOUND_MSG);
            }

            //update crypto balance
            var beforeBalance = cryptoWallet.Balance;
            cryptoWallet.Balance -= amount;

            //add transaction history
            _context.CryptoTransactions.Add(new CryptoTransaction
            {
                Action = CryptoAction.Withdraw,
                Amount = amount,
                BeforeBalance = beforeBalance,
                CryptoWalletId = id,
                Note = "Withdraw to my wallet"
            });

            //update wallet balance
            beforeBalance = wallet.Balance;
            wallet.Balance += amount;

            //add transaction history
            _context.WalletTransactions.Add(new WalletTransaction
            {
                Action = TransactionAction.Receive,
                Amount = amount,
                BeforeBalance = beforeBalance,
                WalletId = id,
                Note = $"Receiving from {cryptoWallet.CryptoCurrency.Name} wallet"
            });

            await _context.SaveChangesAsync();

            return cryptoWallet.Balance;
        }
    }
}
