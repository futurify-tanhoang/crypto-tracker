using CryptoTracker.Authentication;
using CryptoTracker.Middlewares;
using CryptoTracker.Models;
using CryptoTracker.Models.BindingModels;
using CryptoTracker.Models.ViewModels;
using CryptoTracker.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.Controllers
{
    [Authorize]
    [Route("api/wallet/cryptos")]
    public class CryptoWalletsController: Controller
    {
        private IAccountService _accountService;
        private IWalletService _walletService;
        private ICryptoWalletService _cryptoService;

        public CryptoWalletsController(IAccountService accountService, IWalletService walletService, ICryptoWalletService cryptoService)
        {
            _accountService = accountService;
            _walletService = walletService;
            _cryptoService = cryptoService;
        }

        [HttpGet("me")]
        public async Task<List<CryptoWalletViewModel>> AllMyWallet()
        {
            var accountId = User.GetAccountId().Value;
            var account = await _accountService.GetAsync(accountId);

            var cryptos = await _cryptoService.GetByWalletIdAsync(account.WalletId);

            return cryptos.Select(t => new CryptoWalletViewModel { Id = t.Id, Balance = t.Balance, CryptoName = t.CryptoCurrency.Name }).ToList();
        }

        [HttpPost]
        [ValidateModelAttribute]
        public async Task<int> Create(CryptoWalletBindingModel bindingModel)
        {
            var accountId = User.GetAccountId().Value;
            var account = await _accountService.GetAsync(accountId);

            var cryptoWallet = new CryptoWallet { CryptoCurrencyId = bindingModel.CryptoCurrencyId, WalletId = account.WalletId };
            await _cryptoService.CreateAsync(cryptoWallet);

            return cryptoWallet.Id;
        }

        [HttpPut("buy")]
        [ValidateModelAttribute]
        public async Task<double> Buy(BuyCryptoBindingModel bindingModel)
        {
            var balance = await _cryptoService.BuyAsync(bindingModel.Id, bindingModel.Quantity, bindingModel.Price, bindingModel.Note);

            return balance;
        }

        [HttpPut("sell")]
        [ValidateModelAttribute]
        public async Task<double> Sell(SellCryptoBindingModel bindingModel)
        {
            var balance = await _cryptoService.SellAsync(bindingModel.Id, bindingModel.Quantity, bindingModel.Price, bindingModel.Note);

            return balance;
        }

        [HttpPut("withdraw")]
        [ValidateModelAttribute]
        public async Task<double> Withdraw(WithdrawCryptoBindingModel bindingModel)
        {
            var accountId = User.GetAccountId().Value;
            var account = await _accountService.GetAsync(accountId);

            var balance = await _cryptoService.WithdrawAsync(bindingModel.CryptoWalletId, account.WalletId, bindingModel.Price, bindingModel.Quantity);

            return balance;
        }
    }
}
